using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebApplication10.Data;
using WebApplication10.Models;

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

    if (!context.Tables.Any())
    {
        var tables = new[]
        {
            new TableForBooking { Number = 5, Seats = 3 },
            new TableForBooking { Number = 11,Seats = 2},
            new TableForBooking { Number = 15, Seats = 4}
        };
        await context.Tables.AddRangeAsync(tables);
        await context.SaveChangesAsync();

        Console.WriteLine("Столы добавлены в базу.");
    }
    else
    {
        Console.WriteLine("В базе данных уже есть столы.");
    }
}