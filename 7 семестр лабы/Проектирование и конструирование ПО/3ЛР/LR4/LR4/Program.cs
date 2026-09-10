using LR4;
using LR4.Models;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;


/// <summary>
/// Создание и настройка WebApplicationBuilder для веб-приложения.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// ===== РЕГИСТРАЦИЯ КАСТОМНЫХ МЕТРИК =====
var totalRequests = Metrics.CreateCounter("total_requests",
    "Total number of HTTP requests over time",
    new CounterConfiguration
    {
        LabelNames = new[] { "method", "path", "status_code" }
    });

var averageResponseTime = Metrics.CreateGauge("average_response_time_seconds",
    "Average response time in seconds",
    new GaugeConfiguration
    {
        LabelNames = new[] { "method", "path" }
    });

// Для расчета средней скорости
var responseTimeSum = new Dictionary<string, double>();
var responseTimeCount = new Dictionary<string, int>();
var lockObject = new object();

builder.Services.AddMetrics();

/// <summary>
/// Добавление поддержки контроллеров в приложение.
/// </summary>
builder.Services.AddControllers();

/// <summary>
/// Добавление контекста базы данных CarContext с использованием PostgreSQL как провайдера базы данных с заданной строкой подключения.
/// </summary>
builder.Services.AddDbContext<CarContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CarsDB"))
);

/// <summary>
/// Добавление API описаний для автоматической генерации документации.
/// </summary>
builder.Services.AddEndpointsApiExplorer();

/// <summary>
/// Добавление поддержки генерации Swagger-документации для API.
/// </summary>
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<Worker>();

/// <summary>
/// Построение конфигурации приложения.
/// </summary>
var app = builder.Build();

// ===== MIDDLEWARE ДЛЯ СБОРА МЕТРИК =====
app.Use(async (context, next) =>
{
    var startTime = DateTime.UtcNow;
    var method = context.Request.Method;
    var path = context.Request.Path;
    var key = $"{method}_{path}";

    try
    {
        await next();

        var statusCode = context.Response.StatusCode.ToString();
        var duration = (DateTime.UtcNow - startTime).TotalSeconds;

        // 1. Счетчик общего количества запросов
        totalRequests.WithLabels(method, path, statusCode).Inc();

        // 2. Расчет средней скорости ответа
        lock (lockObject)
        {
            if (!responseTimeSum.ContainsKey(key))
            {
                responseTimeSum[key] = 0;
                responseTimeCount[key] = 0;
            }

            responseTimeSum[key] += duration;
            responseTimeCount[key]++;

            var average = responseTimeSum[key] / responseTimeCount[key];
            averageResponseTime.WithLabels(method, path).Set(average);
        }
    }
    catch (Exception)
    {
        // Обработка ошибок
        var duration = (DateTime.UtcNow - startTime).TotalSeconds;
        totalRequests.WithLabels(method, path, "500").Inc();

        lock (lockObject)
        {
            if (!responseTimeSum.ContainsKey(key))
            {
                responseTimeSum[key] = 0;
                responseTimeCount[key] = 0;
            }

            responseTimeSum[key] += duration;
            responseTimeCount[key]++;

            var average = responseTimeSum[key] / responseTimeCount[key];
            averageResponseTime.WithLabels(method, path).Set(average);
        }
        throw;
    }
});

/// <summary>
/// Проверка, работает ли приложение в режиме разработки, и включение Swagger для удобства тестирования.
/// </summary>
if (app.Environment.IsDevelopment())
{
    /// <summary>
    /// Включение Swagger для генерации документации.
    /// </summary>
    app.UseSwagger();

    /// <summary>
    /// Включение Swagger UI для взаимодействия с API через веб-интерфейс.
    /// </summary>
    app.UseSwaggerUI();
}

/// <summary>
/// Включение перенаправления HTTP-запросов на HTTPS.
/// </summary>
app.UseHttpsRedirection();

app.UseRouting();

// Это добавит endpoint /metrics
app.UseHttpMetrics();
app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics(); // /metrics
});

/// <summary>
/// Включение авторизации в приложении.
/// </summary>
app.UseAuthorization();

/// <summary>
/// Настройка маршрутов для контроллеров.
/// </summary>
app.MapControllers();

/// <summary>
/// Запуск веб-приложения.
/// </summary>
app.Run();