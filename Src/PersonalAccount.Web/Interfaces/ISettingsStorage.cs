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
    /// Выбранные настройки.
    /// </summary>
    LoadingSettingsModel Settings { get; set; }

    /// <summary>
    /// Выбранный филиал.
    /// </summary>
    BranchModel Branch { get; }
}
