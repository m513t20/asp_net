using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PersonalAccount.Common.Core;
using PersonalAccount.Data.Extensions;
using PersonalAccount.Data.Logics;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.IntegrationTests;


/*
Имя проверяемого метода
Сценарий, в котором тестируется метод
Ожидаемое поведение при вызове сценария
*/


public class CompanySettingsTests
{

   // Работа с контейнером
    private IServiceProvider _provider;

    public CompanySettingsTests()
    {
       var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

        var configuration = builder.Build();
        var services = new ServiceCollection()
                     .RegistryPersonalAccountData( configuration );

        _provider = services.BuildServiceProvider();
    }


    /// <summary>
    /// Прверить работу метода Load класса <see cref="SettingsRepository"/>
    /// </summary>
    /// <returns></returns>
    [Test]
    [TestCase("655315b0-f7dd-463b-abeb-01ba3f770cac")]
    [Order(2)]
    public void  Load_CompanySettingsRepository_NotThrow(string companyId)
    {
        // Подготова
        var repo = _provider.GetRequiredService<ISettingsRepository>();
        var branch = new BranchModel()
        {
            Id = new Guid( companyId )
        };

        // Проверки и действие
        Assert.DoesNotThrowAsync( async() =>
        {
            var result = await repo.LoadAsync(branch, CancellationToken.None);
            Assert.That(result is not null);
        });
    }

    /// <summary>
    /// Проверить работу метода Save класса <see cref="SettingsRepository"/>
    /// </summary>
    /// <returns></returns>
    [Test]
    [TestCase("655315b0-f7dd-463b-abeb-01ba3f770cac")]
    [Order(1)]
    public void Save_CompanySettingsRepository_NotThrow(string branchId)
    {
        // Подготовка
        var repo = _provider.GetRequiredService<ISettingsRepository>();
        var branch = new BranchModel()
        {
            Id = new Guid( branchId )
        };
        var setting = new LoadingSettingsModel()
        {
            Branch = branch, BatchSize = 10000, StartPosition = 0
        };

        // Действие и проверка
        Assert.DoesNotThrowAsync(async () =>
        {
            await repo.SaveAsync(setting, CancellationToken.None);
            var result = await repo.LoadAsync(branch, CancellationToken.None);

            Assert.That(result?.StartPosition == setting.StartPosition, Is.True);
            Assert.That(result?.BatchSize == setting.BatchSize, Is.True);
        });
    }
}
