using LR4.GraphQL.Types;
using LR4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LR4.GraphQL.Mutations
{
    public class CarMutation
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CarMutation(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<CarType> CreateCar(string brand, string model, int year, string color, decimal price, bool isAvailable, long clientId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CarContext>();

            var car = new Car
            {
                Brand = brand,
                Model = model,
                Year = year,
                Color = color,
                Price = price,
                IsAvailable = isAvailable,
                Confirmed = "pending"
            };

            context.Cars.Add(car);
            await context.SaveChangesAsync();

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

        public async Task<CarType?> UpdateCar(long id, string brand, string model, int year, string color, decimal price, bool isAvailable)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CarContext>();

            var car = await context.Cars.FindAsync(id);
            if (car == null)
                return null;

            car.Brand = brand;
            car.Model = model;
            car.Year = year;
            car.Color = color;
            car.Price = price;
            car.IsAvailable = isAvailable;

            await context.SaveChangesAsync();

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

        public async Task<bool> DeleteCar(long id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CarContext>();

            var car = await context.Cars.FindAsync(id);
            if (car == null)
                return false;

            context.Cars.Remove(car);
            await context.SaveChangesAsync();
            return true;
        }
    }
}