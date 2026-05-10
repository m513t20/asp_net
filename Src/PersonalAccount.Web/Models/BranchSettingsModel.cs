using System;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Web.Models;

/// <summary>
/// Модель данных для формы настроек.
/// </summary>
public class BranchSettingsModel
{
    /// <summary>
    /// Список филиалов.
    /// </summary>
    public List<BranchModel> Branches { get; set; } = null!;

    /// <summary>
    /// Выбранный филиал.
    /// </summary>
    public BranchModel Branch { get; set; } = null!;
}
