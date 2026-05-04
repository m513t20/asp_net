using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PersonalAccount.Api.Extensions;
using PersonalAccount.Common.Core;
using PersonalAccount.Common.Models;
using PersonalAccount.Console.Extensions;
using PersonalAccount.Data;
using PersonalAccount.Data.Extensions;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.IntegrationTests;

/*
Имя проверяемого метода
Сценарий, в котором тестируется метод
Ожидаемое поведение при вызове сценария
*/


/// <summary>
/// Набор интеграционных тестов для проверки работы различных репозиториев.
/// </summary>
public class RepositoryTests
{
    // Работа с контейнером
    private IServiceProvider _provider;

    public RepositoryTests()
    {
        var builder = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json");

        var configuration = builder.Build();
        var services = new ServiceCollection()
                     .RegistryPersonalAccountData(configuration)
                     .RegistryPersonalAccountConsole(configuration)
                     .RegistryPersonalAccountApi(configuration);

        _provider = services.BuildServiceProvider();
    }

    /// <summary>
    /// Простой тест для замера производительности.
    /// </summary>
    /// <param name="rows"></param>
    /// <returns></returns>
    [Test]
    [TestCase(100)]
    [TestCase(1000)]
    [TestCase(10000)]
    [Order(1)]
    public async Task GetRows_JournalRepository_Any(int rows)
    {
        // Подготовка
        var options = _provider.GetRequiredService<ConsoleOptions>();
        var repo = _provider.GetRequiredService<IClientRepository<JournalRowDto>>();
        using var connect = new SqlConnection(options.ConnectionString);


        // Действие
        var result = await repo.GetRows(connect, new Domain.Models.LoadingSettingsModel() { BatchSize = rows });

        // Проверки
        Assert.That(result is not null);
        Assert.That(result!.Any());
        Assert.That(result!.Count() == rows);
    }


    /// <summary>
    /// Проверить работу метода SaveRows репозиторий JournalRepository
    /// </summary>
    [Test]
    [Order(2)]
    public void SaveRows_JournalRepository_DoesNotThrow()
    {
        // Подготовка
        var repo = _provider.GetRequiredService<IServerRepository<JournalRowDto>>();
        var context = _provider.GetRequiredService<PersonalAccountContext>();
        var connect = context.Database.GetDbConnection();
        var transactions = new List<JournalRowDto>()
        {
            new JournalRowDto()
            {
                TypeCode = 101,
                CategoryCode = 1,
                CategoryName = "test",
                Code = 1 ,
                Discount = 0,
                EmploeeCode = 1 ,
                EmploeeName = "test" ,
                Period = DateTime.UtcNow,
                Price = 1,
                ProductCode = 1 ,
                ProductName = "test",
                Quantity = 1,
                ReceiptNumber = 1
            }
        };
        var options = new LoadingSettingsModel()
        {
            StartPosition = 1,
            BatchSize = 1000,

            // select * from branches
            Branch = new BranchModel()
            {
                Id = new Guid("655315b0-f7dd-463b-abeb-01ba3f770cac"),
                Owner = new CompanyModel()
                {
                    Id = new Guid("14e54725-0efc-42b8-a27d-a84f9a7257c5")
                }
            }
        };

        // Действие и проверка
        Assert.DoesNotThrowAsync(async () => await repo.SaveRows(connect, transactions, options));
    }


    /// <summary>
    /// Проверить работу метода GetRows репозиторий CategoryRepository
    /// </summary>
    [Test]
    [Order(3)]
    public void GetRows_CategoryRepository_DoesNotThrow()
    {
        // Подготовка
        var repo = _provider.GetRequiredService<IBufferedRepository<JournalRowDto, CategoryModel>>();
        var transactions = new List<JournalRowDto>()
        {
            new JournalRowDto()
            {
                CategoryCode = 1, CategoryName = "test"
            }
        };
        var options = new LoadingSettingsModel()
        {
            Branch = new BranchModel()
            {
                Id = new Guid("655315b0-f7dd-463b-abeb-01ba3f770cac"),
                Owner = new CompanyModel()
                {
                    Id = new Guid("14e54725-0efc-42b8-a27d-a84f9a7257c5")
                }
            }
        };

        // Действия и проверка
        Assert.DoesNotThrow(() =>
        {
            var result = repo.GetRows(transactions, options);
            Assert.That(result.Any());
            Assert.That(result.Count() == 1);
        });
    }

    /// <summary>
    /// Проверить работу метода Save репозитория CategoryRepository
    /// </summary>
    [Test]
    [Order(4)]
    public void Save_CategoryRepository_DoesNotThrow()
    {
        // Подготовка
        var repo = _provider.GetRequiredService<IBufferedRepository<JournalRowDto, CategoryModel>>();
        var transactions = new List<JournalRowDto>()
        {
            new JournalRowDto()
            {
                CategoryCode = 1, CategoryName = "test1"
            }
        };
        var options = new LoadingSettingsModel()
        {
            Branch = new BranchModel()
            {
                Id = new Guid("655315b0-f7dd-463b-abeb-01ba3f770cac"),
                Owner = new CompanyModel()
                {
                    Id = new Guid("14e54725-0efc-42b8-a27d-a84f9a7257c5")
                }
            }
        };
        var categories = repo.GetRows(transactions, options);

        // Действие и проверка
        Assert.DoesNotThrow(() =>
        {
            Assert.That(categories.Any());
            repo.Save(categories, options);
        });
    }


    /// <summary>
    /// Проверить работу метода GetRows репозитория NomenclatureRepository
    /// </summary>
    [Test]
    [Order(5)]
    public void GetRows_NomenclatureRepository_DoesNotThrow()
    {
        // Подготовка
        var repo = _provider.GetRequiredService<IBufferedRepository<JournalRowDto, NomenclatureModel>>();
        var options = new LoadingSettingsModel()
        {
            Branch = new BranchModel()
            {
                Id = new Guid("655315b0-f7dd-463b-abeb-01ba3f770cac"),
                Owner = new CompanyModel()
                {
                    Id = new Guid("14e54725-0efc-42b8-a27d-a84f9a7257c5")
                }
            }
        };
        var transactions = new List<JournalRowDto>()
        {
            new JournalRowDto()
            {
                CategoryCode = 1, CategoryName = "test", ProductCode = 1, ProductName = "Test"
            }
        };

        // Действия и проверка
        Assert.DoesNotThrow(() =>
        {
            var result = repo.GetRows(transactions, options);
            Assert.That(result.Any());
            Assert.That(result.Count() == 1);
        });
    }


    /// <summary>
    /// Проверить работу метода Push репозитория TransactionRepository
    /// </summary>
    [Test]
    [Order(6)]
    public void Push_TransactionRepository_True()
    {
        var repo = _provider.GetRequiredService<ITransactionRepository>();
        var options = new LoadingSettingsModel()
        {
            Branch = new BranchModel()
            {
                Id = new Guid("655315b0-f7dd-463b-abeb-01ba3f770cac"),
                Owner = new CompanyModel()
                {
                    Id = new Guid("14e54725-0efc-42b8-a27d-a84f9a7257c5")
                }
            }
        };
        var transactions = new List<JournalRowDto>()
        {
            new JournalRowDto()
            {
                CategoryCode = 1,
                CategoryName = "Test",
                ProductCode = 1,
                ProductName = "Test",
                EmploeeCode = 1,
                EmploeeName = "Test",
                Discount = 0,
                Price = 1 ,
                Quantity = 1,
                Period = DateTime.Now,
                Code = 1,
                ReceiptNumber = 1,
                TypeCode = 101
            }
        };

        // Действие
        var result = repo.Push(transactions, options);

        // Проверка
        Assert.That(result == true);
    }
}
