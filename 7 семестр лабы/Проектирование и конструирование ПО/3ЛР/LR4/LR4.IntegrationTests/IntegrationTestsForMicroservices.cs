using k8s.KubeConfigModels;
using LR4.Controllers;
using LR4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Text.Json;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.OidcClient;

namespace LR4.IntegrationTests
{
    public class IntegrationTestsForMicroservices
    {
        private readonly CarContext _context;
        private Process _lr4MicroserviceProcess;
        private Process _lr4ClientsMicroserviceProcess;

        public IntegrationTestsForMicroservices() {
            // Настройка конфигурации для загрузки параметров из appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Получаем строку подключения из конфигурации
            var connectionString = configuration.GetConnectionString("CarsDB");

            var options = new DbContextOptionsBuilder<CarContext>()
                .UseNpgsql(connectionString)
                .Options;

            _context = new CarContext(options);
        }

        [Fact]
        public async Task Create_5_Cars_With_Confirmed_And_5_CarsWithout()
        {
            const int COUNT_CARS_WITH_CONFIRMED = 5;
            // Шаг 1: Очистить базу данных
            await Delete_all_cars_in_db();

            // Шаг 2: Запустить оба микросервиса
            StartMicroservices();

            // Шаг 3: Создать первые 5 машин
            await Create5Cars();

            await WaitForAllDataChanges(() =>
            {
                _context.ChangeTracker.Clear();//Очищает информацию о всех сущностях,
                                               //которые Entity Framework отслеживает в данный момент
                var cars = _context.Cars.AsNoTracking().ToList();
                return cars.All(car => car.Confirmed != null);
            });

            // Шаг 5: Остановить второй микросервис
            StopLR4ClientsMicroservice();

            // Шаг 6: Создать еще 5 машин
            await Create5Cars();

            // Шаг 7: Остановить первый микросервис
            StopLR4Microservice();

            // Шаг 9: Проверить состояние машин
            var carsWithConfirmed = _context.Cars.Where(car => car.Confirmed != null).Count();
            Assert.Equal(COUNT_CARS_WITH_CONFIRMED, carsWithConfirmed); // Убедиться, что только у первых 10 машин есть поле "Confirmed"
        }


        private void StartMicroservices()
        {
            _lr4MicroserviceProcess = StartMicroservice("C:/Users/Максим/source/repos/LR4/LR4/LR4.csproj");
            _lr4ClientsMicroserviceProcess = StartMicroservice("C:/Users/Максим/source/repos/LR4/LR4.Clients/LR4.Clients.csproj");
        }

        private Process StartMicroservice(string projectPath)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"run --project {projectPath}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            return process;
        }

        private void StopLR4Microservice()
        {
            if (!_lr4MicroserviceProcess.HasExited)
            {
                _lr4MicroserviceProcess.Kill();
            }
        }

        private void StopLR4ClientsMicroservice()
        {
            if (!_lr4ClientsMicroserviceProcess.HasExited)
            {
                _lr4ClientsMicroserviceProcess.Kill();
            }

        }

        public async Task Create5Cars()
        {
            var carController = new CarsController(_context);

            const int COUNT_FOR_CREATE_CARS = 5;
            const int ID_FIRST_CLIENT = 1;
            for (int i = 0; i < COUNT_FOR_CREATE_CARS; i++)
            {
                var carDTO = new CarDTO
                {
                    ClientId = ID_FIRST_CLIENT,
                    Brand = $"Brand_{i+1}",
                    Color = $"Color_{i+1}",
                    IsAvailable = true,
                    Model = $"Model_{i + 1}",
                    Year = 1901 + i,
                    Price = 2424 + i
                };
                var result = await carController.PostCar(carDTO);
            }
        }

        public async Task Delete_all_cars_in_db()
        {
            var cars = await _context.Cars.ToListAsync();
            _context.Cars.RemoveRange(cars);
            await _context.SaveChangesAsync();      
        }

        private async Task WaitForAllDataChanges(Func<bool> condition, int pollingInterval = 100)
        {
            while (!condition())
            {
                Console.WriteLine("Waiting for all data changes...");
                await Task.Delay(pollingInterval);
            }
            Console.WriteLine("All data changes detected.");
        }
    }
}

