using Confluent.Kafka;
using LR4.Clients.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace LR4.Clients
{
    internal sealed class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started.");
            var consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = "localhost:9092",
                ClientId = "myconsumerclient",
                GroupId = "MyConsumerGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build())
            {
                _logger.LogInformation("Work!");
                consumer.Subscribe("clientTopic");

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        
                        var consumerData = consumer.Consume(TimeSpan.FromMilliseconds(5));
                        if (consumerData != null)
                        {
                            // Десериализация сообщения
                            var car = JsonSerializer.Deserialize<CarMessage>(consumerData.Message.Value);
                            if (car != null)
                            {
                                _logger.LogInformation($"Consumed message for ClientId: {car.ClientId}");

                                // Создаем scope для работы с ClientContext
                                using (var scope = _serviceProvider.CreateScope())
                                {
                                    var context = scope.ServiceProvider.GetRequiredService<ClientContext>();

                                    if (await ClientExistsAsync(context, car.ClientId))
                                    {
                                        var client = await context.Clients.FindAsync(car.ClientId);
                                        if (client != null)
                                        {
                                            // Увеличиваем количество зарегистрированных объектов
                                            client.RegisteredObjects++;
                                            await context.SaveChangesAsync();
                                        }
                                    }

                                    // Подготовка сообщения для отправки
                                    var resultMessage = new Models.ResultMessage
                                    {
                                        CarId = car.CarId,
                                        UpdateTime = DateTime.Now.ToString()
                                    };

                                    var message = new Message<string, string>()
                                    {
                                        Key = car.ClientId.ToString(),
                                        Value = JsonSerializer.Serialize(resultMessage)
                                    };

                                    // Конфигурация Kafka producer
                                    var producerConfig = new ProducerConfig()
                                    {
                                        BootstrapServers = "localhost:9092",
                                        Acks = Acks.All
                                    };

                                    using (var producer = new ProducerBuilder<string, string>(producerConfig).Build())
                                    {
                                        await producer.ProduceAsync("carsTopic", message, stoppingToken);
                                        _logger.LogInformation($"Message sent to carsTopic for CarId: {car.CarId}");
                                    }
                                }

                            }
                        }
                        else await Task.Delay(10, stoppingToken);
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError($"Error consuming message: {e.Error.Reason}");
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Unexpected error: {e.Message}");
                    }
                }
            }
        }

        private async Task<bool> ClientExistsAsync(ClientContext context, long id)
        {
            return await context.Clients.AnyAsync(client => client.Id == id);
        }
    }
}
