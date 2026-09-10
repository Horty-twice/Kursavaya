using LR4;
using LR4.Models;
using Microsoft.EntityFrameworkCore;
using LR4.GraphQL.Queries;    // Добавьте эти using
using LR4.GraphQL.Mutations;  // для GraphQL

/// <summary>
/// Создание и настройка WebApplicationBuilder для веб-приложения.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

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
/// Настройка GraphQL сервера с поддержкой запросов и мутаций
/// </summary>
builder.Services
    .AddGraphQLServer()
    .AddQueryType<CarQuery>()      // Регистрация Query
    .AddMutationType<CarMutation>(); // Регистрация Mutation

/// <summary>
/// Построение конфигурации приложения.
/// </summary>
var app = builder.Build();

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

/// <summary>
/// Включение авторизации в приложении.
/// </summary>
app.UseAuthorization();

/// <summary>
/// Настройка маршрутов для контроллеров.
/// </summary>
app.MapControllers();

/// <summary>
/// Настройка маршрута для GraphQL endpoint.
/// </summary>
app.MapGraphQL(); // GraphQL будет доступен по /graphql

/// <summary>
/// Запуск веб-приложения.
/// </summary>
app.Run();