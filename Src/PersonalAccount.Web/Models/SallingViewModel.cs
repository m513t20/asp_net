using System;
using PersonalAccount.Domain.Models.Dto;
using PersonalAccount.Web.Interfaces;

namespace PersonalAccount.Web.Models;

/// <summary>
/// Модель отчета продаж.
/// </summary>
public class SallingViewModel : SellingDto, IReportSettings
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
