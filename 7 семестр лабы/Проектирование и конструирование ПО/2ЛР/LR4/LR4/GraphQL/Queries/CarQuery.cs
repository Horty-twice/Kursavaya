using LR4.GraphQL.Types;
using LR4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; // Добавьте этот using

namespace LR4.GraphQL.Queries
{
    public class CarQuery
    {
        private readonly IServiceScopeFactory _serviceScopeFactory; // Используем IServiceScopeFactory

        public CarQuery(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        // Получить все автомобили
        public async Task<List<CarType>> GetCars()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CarContext>();

            var cars = await context.Cars.ToListAsync();
            return cars.Select(c => new CarType
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model,
                Year = c.Year,
                Color = c.Color,
                IsAvailable = c.IsAvailable,
                Price = c.Price,
                Confirmed = c.Confirmed
            }).ToList();
        }

        // Получить автомобиль по ID
        public async Task<CarType?> GetCarById(long id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CarContext>();

            var car = await context.Cars.FindAsync(id);
            if (car == null)
                return null;

            return new CarType
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                Color = car.Color,
                IsAvailable = car.IsAvailable,
                Price = car.Price,
                Confirmed = car.Confirmed
            };
        }

        // Поиск автомобилей по бренду
        public async Task<List<CarType>> GetCarsByBrand(string brand)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CarContext>();

            var cars = await context.Cars
                .Where(c => c.Brand != null && c.Brand.Contains(brand))
                .ToListAsync();

            return cars.Select(c => new CarType
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model,
                Year = c.Year,
                Color = c.Color,
                IsAvailable = c.IsAvailable,
                Price = c.Price,
                Confirmed = c.Confirmed
            }).ToList();
        }

        // Фильтрация по году выпуска
        public async Task<List<CarType>> GetCarsByYear(int minYear, int? maxYear = null)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CarContext>();

            var query = context.Cars.Where(c => c.Year >= minYear);

            if (maxYear.HasValue)
                query = query.Where(c => c.Year <= maxYear.Value);

            var cars = await query.ToListAsync();

            return cars.Select(c => new CarType
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model,
                Year = c.Year,
                Color = c.Color,
                IsAvailable = c.IsAvailable,
                Price = c.Price,
                Confirmed = c.Confirmed
            }).ToList();
        }

        // Автомобили в наличии
        public async Task<List<CarType>> GetAvailableCars()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CarContext>();

            var cars = await context.Cars
                .Where(c => c.IsAvailable)
                .ToListAsync();

            return cars.Select(c => new CarType
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model,
                Year = c.Year,
                Color = c.Color,
                IsAvailable = c.IsAvailable,
                Price = c.Price,
                Confirmed = c.Confirmed
            }).ToList();
        }

        // Автомобили по ценовому диапазону
        public async Task<List<CarType>> GetCarsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CarContext>();

            var cars = await context.Cars
                .Where(c => c.Price >= minPrice && c.Price <= maxPrice)
                .ToListAsync();

            return cars.Select(c => new CarType
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model,
                Year = c.Year,
                Color = c.Color,
                IsAvailable = c.IsAvailable,
                Price = c.Price,
                Confirmed = c.Confirmed
            }).ToList();
        }
    }
}