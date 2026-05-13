using System;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Web.Interfaces;

/// <summary>
/// Интерфейс для хранения данных о настройках и филиалах.
/// </summary>
public interface ISettingsStorage
{
    /// <summary>
    /// Доступные филиалы.
    /// </summary>
    IEnumerable<BranchModel> Branches { get; }

    /// <summary>
    /// Получить настройки по ID ветки.
    /// </summary>
    /// <returns></returns>
    LoadingSettingsModel GetSettingsByBranchId(Guid id);

    /// <summary>
    /// Сохранить настройки загрузки.
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    bool SaveLoadingSettings(LoadingSettingsModel model);
}
