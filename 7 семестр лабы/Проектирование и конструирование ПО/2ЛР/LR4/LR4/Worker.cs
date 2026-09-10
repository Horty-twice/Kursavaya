using Confluent.Kafka;
using LR4.Models;
using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace LR4
{
    internal sealed class Worker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public Worker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Starting background task...");

            var consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = "localhost:9092",
                ClientId = "mycarconsummerclient",
                GroupId = "MyCarConsumerGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build())
            {
                consumer.Subscribe("carsTopic");

                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumerData = consumer.Consume(TimeSpan.FromMilliseconds(5));

                    if (consumerData != null)
                    {
                        var client = JsonSerializer.Deserialize<ResultMessage>(consumerData.Message.Value);

                        // Создаем скоуп для работы с контекстом
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var context = scope.ServiceProvider.GetRequiredService<CarContext>();

                            // Проверяем существование машины
                            if (await CarExistsAsync(context, client.CarId))
                            {
                                var car = await context.Cars.FindAsync(client.CarId);
                                if (car != null)
                                {
                                    car.Confirmed = client.UpdateTime;
                                    await context.SaveChangesAsync();
                                }
                            }
                        }
                    }

                    else await Task.Delay(1, stoppingToken);
                }
            }
        }

        // Асинхронная проверка существования машины
        private async Task<bool> CarExistsAsync(CarContext context, long id)
        {
            return await context.Cars.AnyAsync(car => car.Id == id);
        }
    }
}
