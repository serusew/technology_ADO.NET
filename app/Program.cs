using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

string? connectionString = configuration.GetConnectionString("CoffeeShop");

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine("Ошибка: строка подключения CoffeeShop не найдена в конфигурации.");
    return;
}

try
{
    await using SqlConnection connection = new SqlConnection(connectionString);
    await connection.OpenAsync();

    using SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM customers", connection);
    int count = (int)(await command.ExecuteScalarAsync());

    Console.WriteLine($"Клиентов в кофейне: {count}");
}
catch (SqlException ex)
{
    Console.WriteLine($"Ошибка SQL Server: {ex.Message}");
}