namespace CityLibrary.DTO
{
    public class ReaderStatusCheck
    {
        public bool IsBlocked { get; set; }
        public bool HasOverdue { get; set; }
        public int ActiveIssuesCount { get; set; }
    }

    public class ReturnInfo
    {
        public int ItemId { get; set; }
        public int IssueId { get; set; }
        public int ReaderId { get; set; }
        public int BookId { get; set; }
        public DateOnly PlannedReturnDate { get; set; }
        public DateOnly? ActualReturnDate { get; set; }
    }

    public class GenreReportItem
    {
        public string GenreName { get; set; }
        public long BorrowCount { get; set; }
    }

    public class OverdueReportItem
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public long DebtorsCount { get; set; }
    }

    public class ReaderInfoDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string StatusName { get; set; }
        public int ActiveIssues { get; set; }
        public int UnpaidFines { get; set; }
    }

    public class BookInfoDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int AvailableCount { get; set; }
        public int TotalCount { get; set; }
    }



}
