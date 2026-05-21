using System;
using System.Transactions;
using PersonalAccount.Domain.Core;
using PersonalAccount.Web.Enums;

namespace PersonalAccount.Web.Interfaces;

/// <summary>
/// Общий интерфейс для создания отчетов.
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Загруженные транзакции.
    /// </summary>
    public IEnumerable<Transaction> Transactions { get; set; }

    /// <summary>
    /// Тип отчета
    /// </summary>
    public ReportTypeEnum ReportType { get; }

    /// <summary>
    /// Получить отчет.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IDto> Get();

    /// <summary>
    /// Создать отчет.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IDto> Create();
}
