using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using PersonalAccount.Data.Models;
using PersonalAccount.Domain.Core.Interfaces;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Data.Logics;

public class LoadingSettingsRepository : ILoadSettingsRepository
{
    /// <summary>
    /// Загрузить настройки.
    /// </summary>
    /// <param name="organisation"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<LoadSettings> LoadAsync(Domain.Models.Organisation organisation, CancellationToken cancellationToken)
    {
        var context = new PersonalAccountContext();
        var item = context.Organisations.FirstOrDefault( x=> x.Id == organisation.Id )
            ?? throw new InvalidDataException($"Organisation not found {organisation.Id}");
        var json = !string.IsNullOrEmpty(item.LoadOptions) ? item.LoadOptions 
            : throw new InvalidDataException($"string is empty");

        var result = JsonSerializer.Deserialize<LoadSettings>(json)
            ?? throw new InvalidDataException($"string is empty");
        return result!;
    }

    /// <summary>
    /// Сохранить настройки
    /// </summary>
    /// <param name="loadSettings"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<bool> SaveAsync(LoadSettings loadSettings, CancellationToken cancellationToken)
    {
        var context = new PersonalAccountContext();
        var item = context.Organisations.FirstOrDefault( x=> x.Id == loadSettings.UserOrganisation.Id )
            ?? throw new InvalidDataException($"Organisation not found {loadSettings.UserOrganisation.Id}");

        var result = JsonSerializer.Serialize(loadSettings);
        item.LoadOptions = result;
        context.SaveChanges();
        return Task.FromResult(true);
    }
}
