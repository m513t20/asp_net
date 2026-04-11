using System;
using NUnit.Framework;
using PersonalAccount.Data.Logics;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.UnitTests;

/// <summary>
/// Тесты загрузки
/// </summary>
public class LoadingSettingsTests
{
    [Test]
    public async Task Load_LoadSettingsRepository_NotThrow()
    {
        // Подготовка
        var repo = new LoadingSettingsRepository();
        var organisation = new Organisation(){ Id = new Guid("5eefff0e-b7dc-4c07-9b71-9115873bb8f3") };

        // Действие и проверка
        Assert.DoesNotThrowAsync( async() =>
        {
            var result = await repo.LoadAsync(organisation, CancellationToken.None);
            Assert.That(result is not null);
        });
    }

    [Test]
    public async Task Save_LoadSettingsRepository_NotThrow()
    {
        // Подготовка
        var repo = new LoadingSettingsRepository();
        var organisation = new Organisation(){ Id = new Guid("5eefff0e-b7dc-4c07-9b71-9115873bb8f3") };
        var loadSettings = new LoadSettings(){ Id = new Guid(), PackSize = 10, LoadDataType = Domain.Models.Enums.LoadDataTypes.CSV, UserOrganisation = organisation};

        // Действие и проверка
        Assert.DoesNotThrowAsync( async() =>
        {
            var isSaved = await repo.SaveAsync(loadSettings, CancellationToken.None);
            var result = await repo.LoadAsync(organisation, CancellationToken.None);
            Assert.That(result is not null);
        });
    }
}
