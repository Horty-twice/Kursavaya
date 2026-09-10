using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LR4.Models;
using Confluent.Kafka;
using System.Text.Json;

namespace LR4.Controllers
{
    [Route("api/[controller]")] // Определяет базовый маршрут для API (api/Cars).
    [ApiController] // Указывает, что этот контроллер — API контроллер (не возвращает представления).
    public class CarsController : ControllerBase
    {
        private readonly CarContext _context; // Контекст базы данных для доступа к таблице Cars.

        /// <summary>
        /// Конструктор контроллера <see cref="CarsController"/>.
        /// Инициализирует контекст БД
        /// </summary>
        /// <param name="context">Контекст БД с которой будем работать</param>
        public CarsController(CarContext context)
        {
            _context = context;
        }

        // GET: api/Cars

        /// <summary>
        /// Возвращает список всех автомобилей из базы данных.
        /// </summary>
        /// <returns>Список объектов Car</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            return await _context.Cars.ToListAsync();
        }

        // GET: api/Cars/5

        /// <summary>
        /// Возвращает автомобиль по его уникальному идентификатору (id).
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <returns>Объект Car или статус 404, если не найден</returns>        
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(long id)
        {
            var car = await _context.Cars.FindAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            return car;
        }

        // PUT: api/Cars/5

        /// <summary>
        /// Обновляет данные автомобиля с указанным идентификатором.
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <param name="carDTO">Обновлённый объект CarDTO</param>
        /// <returns>Статус 204 при успешном обновлении или статус 404 при ошибке</returns>

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(long id, CarDTO carDTO)
        {
            var car = await _context.Cars.FindAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            car.Brand = carDTO.Brand;
            car.Year = carDTO.Year;
            car.Color = carDTO.Color;
            car.Model = carDTO.Model;
            car.IsAvailable = carDTO.IsAvailable;
            car.Price = carDTO.Price;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!CarExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Cars

        /// <summary>
        /// Добавляет новый автомобиль в базу данных.
        /// </summary>
        /// <param name="carDTO">Объект CarDTO для добавления</param>
        /// <returns>Статус 201 и созданный объект Car</returns>

        [HttpPost]
        public async Task<ActionResult<CarDTO>> PostCar(CarDTO carDTO)
        {
            var car = new Car
            {
                Brand = carDTO.Brand,
                Color = carDTO.Color,
                IsAvailable = carDTO.IsAvailable,
                Model = carDTO.Model,
                Year = carDTO.Year,
                Price = carDTO.Price
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            CarMessage carMessage = new CarMessage { CarId = car.Id,ClientId = carDTO.ClientId};
            var message = new Message<string, string>()
            {
                Key = car.Id.ToString(),
                Value = JsonSerializer.Serialize(carMessage)
            };

            var producerConfig = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092",
                Acks = Acks.All
            };

            var producer = new ProducerBuilder<string, string>(producerConfig).Build();
            await producer.ProduceAsync("clientTopic", message);
            producer.Dispose();



            return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
        }

        [HttpPost("new100Cars")]
        public async Task Create_and_add_100_cars_in_db()
        {
            const int COUNT_NEW_CAR = 100;
            const int ID_FIRST_CLIENT = 1;
            for (int i = 0; i < COUNT_NEW_CAR; i++)
            {

                var carDTO = new CarDTO
                {
                    ClientId = ID_FIRST_CLIENT,
                    Brand = $"Brand_{i + 1}",
                    Color = $"Color_{i + 1}",
                    IsAvailable = true,
                    Model = $"Model_{i + 1}",
                    Year = 1901 + i,
                    Price = 2424 + i
                };
                await PostCar(carDTO);

            }
            await _context.SaveChangesAsync();
        }
        // DELETE: api/Cars/5

        /// <summary>
        /// Удаляет автомобиль с указанным идентификатором.
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <returns>Статус 204 при успешном удалении или 404, если не найден</returns>

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(long id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Проверяет, существует ли автомобиль с указанным идентификатором.
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <returns>true, если автомобиль существует; false, если нет</returns>
        private bool CarExists(long id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }

        /// <summary>
        /// Создаёт объект класса CarDTO на основе некоторых данных объекта класса Car
        /// </summary>
        /// <param name="car">Объект Car откуда берутся данные</param>
        /// <returns>Объект класса CarDTO</returns>
        private static CarDTO CarDTO(Car car) =>
           new CarDTO
           {
               Brand = car.Brand,
               Color = car.Color,
               IsAvailable = car.IsAvailable,
               Model = car.Model,
               Year = car.Year,
               Price = car.Price
           };
    }
}
