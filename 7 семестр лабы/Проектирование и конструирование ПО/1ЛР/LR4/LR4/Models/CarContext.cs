using Microsoft.EntityFrameworkCore;

namespace LR4.Models
{
    /// <summary>
    /// Контекст базы данных для работы с сущностями типа <see cref="Car"/>.
    /// Наследуется от <see cref="DbContext"/>, предоставляя доступ к базе данных и ее таблицам.
    /// </summary>
    public class CarContext : DbContext
    {
        /// <summary>
        /// Представляет коллекцию автомобилей в базе данных.
        /// </summary>
        public DbSet<Car> Cars { get; set; } = null!;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CarContext"/> с заданными параметрами контекста базы данных.
        /// </summary>
        /// <param name="options">Параметры конфигурации для контекста базы данных.</param>
        public CarContext(DbContextOptions<CarContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
