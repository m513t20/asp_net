using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonalAccount.Domain.Models.Dto;
using PersonalAccount.Web.Enums;

namespace PersonalAccount.Web.Models;

/// <summary>
/// Модель для отображения отчетов.
/// </summary>
public class ReportFilterViewModel
{
    /// <summary>
    /// Целевой филиал.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Начало отчета.
    /// </summary>
    public DateTime Start { get; set; } = DateTime.Today.AddDays(-7);

    /// <summary>
    /// Конец отчета.
    /// </summary>
    public DateTime End { get; set; } = DateTime.Today.AddDays(1);

    /// <summary>
    /// Тип отчета.
    /// </summary>
    public ReportTypeEnum ReportType { get; set; }

    /// <summary>
    /// Список ветвей на выбор.
    /// </summary>
    public IEnumerable<SelectListItem> Branches { get; set; } = new List<SelectListItem>();
    
    /// <summary>
    /// Сформированный отчет выручки.
    /// </summary>
    public IEnumerable<RevenueDto>? RevenueReport { get; set; }

    /// <summary>
    /// Сформированный отчет продаж.
    /// </summary>
    public IEnumerable<SellingDto>? SellingReport { get; set; }

    /// <summary>
    /// Сформированный отчет графика работы.
    /// </summary>
    public IEnumerable<WorkScheduleDto>? WorkScheduleReport { get; set; }
}
