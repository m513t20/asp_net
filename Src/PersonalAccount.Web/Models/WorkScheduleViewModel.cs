using System;
using PersonalAccount.Domain.Models.Dto;
using PersonalAccount.Web.Interfaces;

namespace PersonalAccount.Web.Models;

/// <summary>
/// Модель отчета графика работы.
/// </summary>
public class WorkScheduleViewModel : WorkScheduleDto, IReportSettings
{
    /// <summary>
    /// Id ветки.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Начало периода.
    /// </summary>
    DateTime IReportSettings.Start
    {
        get => Start.DateTime;
        set => Start = new DateTimeOffset(value);
    }

    /// <summary>
    /// Конец периода.
    /// </summary>
    DateTime IReportSettings.Stop
    {
        get => Stop.DateTime;
        set => Stop = new DateTimeOffset(value);
    }
}
