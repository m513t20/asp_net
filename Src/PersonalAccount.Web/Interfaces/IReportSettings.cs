using System;

namespace PersonalAccount.Web.Interfaces;

/// <summary>
/// Общий интерфейс для моделей DTO отчетов.
/// </summary>
public interface IReportSettings
{
    /// <summary>
    /// Id ветки.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Начало периода.
    /// </summary>
    public DateTime Start { get; set; }

    /// <summary>
    /// Конец периода.
    /// </summary>
    public DateTime Stop { get; set; }
}
