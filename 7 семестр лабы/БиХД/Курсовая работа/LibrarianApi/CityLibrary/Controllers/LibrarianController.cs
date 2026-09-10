using CityLibrary.Models;
using CityLibrary.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CityLibrary.LibrarianApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrarianController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public LibrarianController(LibraryDbContext context)
        {
            _context = context;
        }

        // =================================================================================
        // ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ (Скрыты из Swagger)
        // Реализуют Use Cases: "Прочитать данные читателя" и "Проверить наличие книги"
        // =================================================================================

        [HttpGet("reader/{id}")]
        [ApiExplorerSettings(IgnoreApi = true)] // <--- Скрывает метод из Swagger
        public async Task<IActionResult> GetReaderInfo(int id)
        {
            var query = @"
                SELECT 
                    r.id AS ""Id"",
                    r.last_name || ' ' || r.first_name || ' ' || r.middle_name AS ""FullName"",
                    r.phone AS ""Phone"",
                    rs.name AS ""StatusName"",
                    (SELECT COUNT(*) FROM issue WHERE readerid = r.id AND issue_statusid = 1) AS ""ActiveIssues"",
                    (SELECT COUNT(*) FROM fine WHERE readerid = r.id AND fine_statusid = 1) AS ""UnpaidFines""
                FROM reader r
                JOIN reader_status rs ON r.reader_statusid = rs.id
                WHERE r.id = {0}";

            var reader = await _context.Database
                .SqlQuery<ReaderInfoDTO>(FormattableStringFactory.Create(query, id))
                .FirstOrDefaultAsync();

            if (reader == null) return NotFound("Читатель не найден");
            return Ok(reader);
        }

        [HttpGet("book/{id}")]
        [ApiExplorerSettings(IgnoreApi = true)] // <--- Скрывает метод из Swagger
        public async Task<IActionResult> GetBookInfo(int id)
        {
            var query = @"
                SELECT 
                    b.id AS ""Id"",
                    b.title AS ""Title"",
                    g.name AS ""Genre"",
                    b.available_quantity AS ""AvailableCount"",
                    b.total_quantity AS ""TotalCount""
                FROM book b
                JOIN genre g ON b.genreid = g.id
                WHERE b.id = {0}";

            var book = await _context.Database
                .SqlQuery<BookInfoDTO>(FormattableStringFactory.Create(query, id))
                .FirstOrDefaultAsync();

            if (book == null) return NotFound("Книга не найдена");
            return Ok(book);
        }

        // =================================================================================
        // ОСНОВНЫЕ ПРОЦЕССЫ (Видны в Swagger)
        // =================================================================================

        // 1. Оформление выдачи книги (Процесс №1 + №6)
        [HttpPost("issue-book")]
        public async Task<IActionResult> IssueBook(int readerId, int bookId, int librarianId)
        {
            // ШАГ 1: Проверка лимитов (SQL)
            // Используем кавычки "", чтобы Postgres не понижал регистр букв в названиях колонок
            var checkQuery = @"
                SELECT 
                    (r.reader_statusid = 2) AS ""IsBlocked"",
                    EXISTS (
                        SELECT 1 FROM issue i_sub 
                        JOIN issue_item ii_sub ON i_sub.id = ii_sub.issueid
                        WHERE i_sub.readerid = r.id 
                          AND ii_sub.actual_return_date IS NULL 
                          AND i_sub.planned_return_date < CURRENT_DATE
                    ) AS ""HasOverdue"",
                    (SELECT COUNT(*) FROM issue WHERE readerid = r.id AND issue_statusid = 1) AS ""ActiveIssuesCount""
                FROM reader r
                WHERE r.id = {0}";

            var status = await _context.Database
                .SqlQuery<ReaderStatusCheck>(FormattableStringFactory.Create(checkQuery, readerId))
                .FirstOrDefaultAsync();

            if (status == null) return NotFound("Читатель не найден");
            if (status.IsBlocked) return BadRequest("ОТКАЗ: Читатель заблокирован.");
            if (status.HasOverdue) return BadRequest("ОТКАЗ: У читателя есть просроченные книги.");
            if (status.ActiveIssuesCount >= 3) return BadRequest("ОТКАЗ: Превышен лимит активных выдач (макс. 3).");

            // ШАГ 2: Выполнение транзакции выдачи
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var requestDate = DateOnly.FromDateTime(DateTime.Now);
                var plannedDate = DateOnly.FromDateTime(DateTime.Now.AddDays(14));

                // Вставка в issue и получение ID
                var issueIds = _context.Database
                                    .SqlQuery<int>($@"
                        INSERT INTO issue (readerid, employeeid, request_date, planned_return_date, issue_statusid)
                        VALUES ({readerId}, {librarianId}, {requestDate}, {plannedDate}, 1) 
                        RETURNING id")
                                    .AsEnumerable(); // <--- ЭТО РЕШАЕТ ОШИБКУ "Non-composable SQL"

                var issueId = issueIds.Single();
                // Вставка позиции выдачи
                await _context.Database.ExecuteSqlAsync($@"
                    INSERT INTO issue_item (issueid, bookid, issue_item_statusid)
                    VALUES ({issueId}, {bookId}, 1)");

                // Обновление количества книг
                var rowsAffected = await _context.Database.ExecuteSqlAsync($@"
                    UPDATE book 
                    SET available_quantity = available_quantity - 1 
                    WHERE id = {bookId} AND available_quantity > 0");

                if (rowsAffected == 0)
                {
                    await transaction.RollbackAsync();
                    return BadRequest("ОТКАЗ: Книги нет в наличии.");
                }

                await transaction.CommitAsync();
                return Ok(new { Message = "Книга успешно выдана", IssueId = issueId });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Ошибка БД: {ex.Message}");
            }
        }

        // 2. Оформление возврата книги (Процесс №2 + №3 + №7)
        [HttpPost("return-book")]
        public async Task<IActionResult> ReturnBook(int issueItemId, bool isDamaged)
        {
            // Получаем информацию о выдаче
            var infoQuery = @"
                SELECT 
                    ii.id AS ""ItemId"",
                    ii.issueid AS ""IssueId"",
                    ii.bookid AS ""BookId"",
                    i.readerid AS ""ReaderId"",
                    i.planned_return_date AS ""PlannedReturnDate"",
                    ii.actual_return_date AS ""ActualReturnDate""
                FROM issue_item ii
                JOIN issue i ON ii.issueid = i.id
                WHERE ii.id = {0}";

            var info = await _context.Database
                .SqlQuery<ReturnInfo>(FormattableStringFactory.Create(infoQuery, issueItemId))
                .FirstOrDefaultAsync();

            if (info == null) return NotFound("Запись о выдаче не найдена");
            if (info.ActualReturnDate != null) return BadRequest("Эта книга уже возвращена.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var today = DateOnly.FromDateTime(DateTime.Now);

                // Фиксация возврата
                await _context.Database.ExecuteSqlAsync($@"
                    UPDATE issue_item 
                    SET actual_return_date = {today}, 
                        issue_item_statusid = 2 
                    WHERE id = {issueItemId}");

                // Автообновление статуса книги (на полку или списание)
                if (!isDamaged)
                {
                    await _context.Database.ExecuteSqlAsync($@"
                        UPDATE book 
                        SET available_quantity = available_quantity + 1 
                        WHERE id = {info.BookId}");
                }
                else
                {
                    // Штраф за порчу (1500 руб, причина 2)
                    await _context.Database.ExecuteSqlAsync($@"
                        INSERT INTO fine (readerid, issue_itemid, fine_reasonid, fine_statusid, amount, imposition_date)
                        VALUES ({info.ReaderId}, {issueItemId}, 2, 1, 1500.00, {today})");
                }

                // Штраф за просрочку (автоматически)
                if (info.PlannedReturnDate < today)
                {
                    await _context.Database.ExecuteSqlAsync($@"
                        INSERT INTO fine (readerid, issue_itemid, fine_reasonid, fine_statusid, amount, imposition_date)
                        VALUES ({info.ReaderId}, {issueItemId}, 1, 1, 100.00, {today})");
                }

                // Закрытие выдачи (если все книги возвращены)
                var pendingCount = await _context.Database
                    .SqlQuery<int>($"SELECT COUNT(*) AS \"Value\" FROM issue_item WHERE issueid = {info.IssueId} AND actual_return_date IS NULL")
                    .SingleAsync();

                if (pendingCount == 0)
                {
                    await _context.Database.ExecuteSqlAsync($@"
                        UPDATE issue 
                        SET issue_statusid = 2 
                        WHERE id = {info.IssueId}");
                }

                await transaction.CommitAsync();
                return Ok(new { Message = isDamaged ? "Книга принята с пометкой 'Повреждена'." : "Книга успешно возвращена." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Ошибка БД: {ex.Message}");
            }
        }

        // 3. Отчет по популярным жанрам (Процесс №4)
        [HttpGet("popular-genres")]
        public async Task<IActionResult> GetPopularGenres()
        {
            var query = @"
                SELECT 
                    g.name AS ""GenreName"",
                    COUNT(ii.id) AS ""BorrowCount""
                FROM issue_item ii
                JOIN book b ON ii.bookid = b.id
                JOIN genre g ON b.genreid = g.id
                GROUP BY g.name
                ORDER BY ""BorrowCount"" DESC
                LIMIT 10";

            var report = await _context.Database
                .SqlQuery<GenreReportItem>(FormattableStringFactory.Create(query))
                .ToListAsync();

            return Ok(report);
        }

        // 4. Отчет по должникам (Процесс №5)
        [HttpGet("overdue-stats")]
        public async Task<IActionResult> GetOverdueStats()
        {
            var query = @"
                SELECT 
                    CAST(EXTRACT(YEAR FROM i.planned_return_date) AS INTEGER) AS ""Year"",
                    CAST(EXTRACT(MONTH FROM i.planned_return_date) AS INTEGER) AS ""Month"",
                    COUNT(DISTINCT i.readerid) AS ""DebtorsCount""
                FROM issue i
                JOIN issue_item ii ON i.id = ii.issueid
                WHERE ii.actual_return_date IS NULL 
                  AND i.planned_return_date < CURRENT_DATE
                GROUP BY ""Year"", ""Month""
                ORDER BY ""Year"", ""Month""";

            var stats = await _context.Database
                .SqlQuery<OverdueReportItem>(FormattableStringFactory.Create(query))
                .ToListAsync();

            return Ok(stats);
        }
    }

}