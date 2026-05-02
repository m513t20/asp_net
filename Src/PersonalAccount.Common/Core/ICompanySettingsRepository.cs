using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Репозиторий для работы с настройками загрузки данных
/// </summary>
public interface ICompanySettingsRepository
{
    /// <summary>
    /// Сохранить настройки
    /// </summary>
    /// <param name="setting"></param>
    public void Save(LoadingSettingsModel setting);

    /// <summary>
    /// Загрузить настройки.
    /// </summary>
    /// <param name="branch"> Филиал </param>
    /// <returns></returns>
    public LoadingSettingsModel? Load(BranchModel branch);

    /// <summary>
    /// Сохранить настройки
    /// </summary>
    /// <param name="setting"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task SaveAsync(LoadingSettingsModel setting,  CancellationToken token);

    /// <summary>
    /// Загрузить настройки
    /// </summary>
    /// <param name="branch"> Филиал </param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<LoadingSettingsModel?> LoadAsync(BranchModel branch, CancellationToken token);
}
