using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PersonalAccount.Console.Extensions;
using PersonalAccount.Data.Models;

namespace PersonalAccount.UnitTests;

/// <summary>
/// Набор тестов для проверки работы контекста базы данных.
/// </summary>
public class DbConnectionTests
{
    private readonly SqlConnection _connection;
    private ServiceProvider _provider;
    private IServiceScope _scope;

    public DbConnectionTests()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
        var configuration = builder.Build();

        var services = new ServiceCollection()
                     .RegistrytPersonalAccountData( configuration );

        _provider = services.BuildServiceProvider();        
        _scope = _provider.CreateScope();
        _connection = _scope.ServiceProvider.GetRequiredService<SqlConnection>();
    }

    /// <summary>
    /// Проверить выборку данных. Получить 10 записей.
    /// </summary>
    /// <returns></returns>
    [Test]
    public void CheckConnection_MSDB_Any()
    {
        // Подготовка
        _connection.Open();
        var sql = "select Top 10 * from journal";
        var command = new SqlCommand(sql, _connection);

        // Действие
        var adapter = new SqlDataAdapter(command);
        var dataset = new DataSet();
        adapter.Fill(dataset);

        // Проверка
        Assert.That(dataset, Is.Not.Null);
        // Assert.That(dataset);
    }
}
