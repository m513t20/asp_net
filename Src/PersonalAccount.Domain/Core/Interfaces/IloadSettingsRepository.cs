using System;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Domain.Core.Interfaces;

/// <summary>
/// Репозиторий для работы с настройками загрузки данных.
/// </summary>
public interface ILoadSettingsRepository
{
    /// <summary>
    /// Сохранить настройки.
    /// </summary>
    /// <param name="loadSettings"></param>
    /// <returns></returns>
    public Task<bool> Save(LoadSettings loadSettings, CancellationToken cancellationToken);

    /// <summary>
    /// Загрузить настройки.
    /// </summary>
    /// <param name="organisation"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<LoadSettings> Load(Organisation organisation, CancellationToken cancellationToken);
}
