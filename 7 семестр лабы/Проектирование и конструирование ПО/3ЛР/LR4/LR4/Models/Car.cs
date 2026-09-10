using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LR4.Models
{
    [Table("cars")]
    public class Car
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column("id")]
        public long Id { get; set; }  // Уникальный идентификатор автомобиля

        [Column("brand")]
        public string? Brand { get; set; }  // Бренд автомобиля (например, BMW, Audi)

        [Column("model")]
        [StringLength(100)]
        public string? Model { get; set; }  // Модель автомобиля (например, X5, A6)

        [Column("year")]
        [Range(1900, 2024)]
        public int Year { get; set; }  // Год выпуска

        [Column("color")]
        [MinLength(2)]
        public string? Color { get; set; }  // Цвет автомобиля

        [Column("IsAvailable")]
        public bool IsAvailable { get; set; }  // Наличие автомобиля в продаже

        [Column("price")]
        public decimal Price { get; set; }  // Цена автомобиля

        [Column("confirmed")]
        public string? Confirmed { get; set; }  
    }

}
