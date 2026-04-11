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
public class DbContextTests
{
    // Работа с контейнером
    private IServiceProvider _provider;

    public DbContextTests()
    {
        var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
        var configuration = builder.Build();

        var services = new ServiceCollection()
                     .RegistrytPersonalAccountApi( configuration );

        _provider = services.BuildServiceProvider();
    }

    /// <summary>
    /// Проверить выборку данных. Получить список всех организаций.
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task FetchCompanies_PersonalAccountContext_Any()
    {
        // Подготовка
        var context = _provider.GetRequiredService<PersonalAccountContext>();
        
        // Действие
        var result = await context.Organisations.ToListAsync(CancellationToken.None);

        // Проверка
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Any());
    }
}
