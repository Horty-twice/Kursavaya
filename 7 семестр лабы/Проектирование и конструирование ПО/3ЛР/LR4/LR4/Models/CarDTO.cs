using System.ComponentModel.DataAnnotations;

namespace LR4.Models
{
    /// <summary>
    /// Entity-класс CarDTO
    /// </summary>
    public class CarDTO
    {
        public long ClientId { get; set; } 
        public string? Brand { get; set; }  // Бренд автомобиля (например, BMW, Audi)

        [StringLength(100)]
        public string? Model { get; set; }  // Модель автомобиля (например, X5, A6)

        [Range(1900, 2024)]
        public int Year { get; set; }  // Год выпуска

        [MinLength(2)]
        public string? Color { get; set; }  // Цвет автомобиля
        public bool IsAvailable { get; set; }  // Наличие автомобиля в продаже
        public decimal Price { get; set; }  // Цена автомобиля

    }
}
