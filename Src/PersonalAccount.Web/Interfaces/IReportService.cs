using PersonalAccount.Domain.Core;
using PersonalAccount.Domain.Models;
using PersonalAccount.Web.Enums;

namespace PersonalAccount.Web.Interfaces;

/// <summary>
/// Общий интерфейс для создания отчетов.
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Сохраненные транзакции
    /// </summary>
    public IEnumerable<TransactionModel> Transactions { get; set; }

    /// <summary>
    /// Получить транзакции.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<TransactionModel> Get(Guid branchId, DateTime start, DateTime end);

    /// <summary>
    /// Создать отчет.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IDto> Create(IEnumerable<TransactionModel> transactions, ReportTypeEnum reportType);
}
