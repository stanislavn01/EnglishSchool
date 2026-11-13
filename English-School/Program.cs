using EnglishSchool.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = configuration
    .GetConnectionString("DefaultConnection");

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseNpgsql(connectionString)
    .Options;

using var dbContext = new AppDbContext(options);

if (dbContext.Database.CanConnect())
{
    Console.WriteLine("Успешное подключение к PostgreSQL!");
}
else
{
    Console.WriteLine("Не удалось подключиться к PostgreSQL.");
}