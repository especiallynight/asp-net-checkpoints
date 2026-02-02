using Microsoft.EntityFrameworkCore;
using System.Linq; 
using WebApplication7.Data;
using WebApplication7.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("YourConnectionString");

builder.Services.AddDbContext<AppDbContext>(options =>  
    options.UseSqlServer(connectionString));

// Add services to the container

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await SeedDataAsync(app);

app.Run();

async Task SeedDataAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await context.Database.MigrateAsync();

    if (!context.Users.Any())
    {
        var users = new[]
        {
            new User { Name = "Александр Петров", Email = "alexandr3215@gmail.com", Age = 23 },
            new User { Name = "Мария Иванова", Email = "maria_ivanova41@mail.ru", Age = 14 },
            new User { Name = "Сергей Сидоров", Email = "sergei_89@bk.ru", Age = 45 }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        Console.WriteLine("Пользователи добавлены в базу.");
    }
    else
    {
        Console.WriteLine("В базе данных уже есть пользователи.");
    }
}