using PersonalAccount.Api.Logics;
using PersonalAccount.Data;
using PersonalAccount.Data.Models;
using PersonalAccount.Domain.Core;
using PersonalAccount.Domain.Models;
using PersonalAccount.Web.Enums;
using PersonalAccount.Web.Interfaces;

namespace PersonalAccount.Web.Logics;

/// <summary>
/// Фабрика для формирования отчетов.
/// </summary>
/// <param name="context"></param>
public class ReportService(PersonalAccountContext context) : IReportService
{
    // Контекст для работы с базой данных
    private readonly PersonalAccountContext _context = context;

    public IEnumerable<TransactionModel> Transactions { get; set; }

    /// <summary>
    /// Создать отчет.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IDto> Create(IEnumerable<TransactionModel> transactions, ReportTypeEnum reportType)
    {
        return reportType switch
        {
            ReportTypeEnum.Revenue => new RevenueReportService().Create(transactions),
            ReportTypeEnum.Selling => new SalesReportService().Create(transactions),
            ReportTypeEnum.WorkSchedule => new WorkScheduleReportService().Create(transactions),
            _ => throw new NotImplementedException()
        };
    }

    /// <summary>
    /// Получить транзакции.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<TransactionModel> Get(Guid branchId, DateTime start, DateTime end)
    {
        var dbBranch = _context.Branches
            .First(x => x.Id == branchId);
        var branch = new BranchModel(){
            Id = dbBranch.Id,
            Name = dbBranch.Name ?? "",
        };

        var categories = _context.Categories
            .Select(x => new CategoryModel(){
                Id = x.Id,
                Name = x.Name ?? "",
                ExternalCode = x.ExternalCode,
            })
            .ToDictionary(x => x.Id);

        var employers  = _context.Emploees
            .Where(x => x.CompanyId == branchId)
            .Select(x => new EmploeeModel() {
                Id = x.Id,
                Name = x.Name ?? "",
                ExternalCode = x.ExternalCode,
                Phone = x.Phone,
            })
            .ToDictionary(x => x.Id);

        var dbNomenclature = _context.Nomenclatures.ToList();
        var nomenclature = dbNomenclature
            .Select(x => new NomenclatureModel()
                {
                    Id = x.Id,
                    Name = x.Name ?? "",
                    ExternalCode = x.ExternalCode,
                    Category = categories.GetValueOrDefault(x.CategoryId ?? Guid.Empty) 
                })
            .ToDictionary(n => n.Id);

        var transactions = _context.Transactions
            .Where(x => x.ChangePeriod > start)
            .Where(x => x.ChangePeriod < end)
            .Where(x => x.BranchId == branchId)
            .ToList();        
    
        return transactions
            .Select(x => new TransactionModel()
                {
                    Id = Guid.NewGuid(),
                    Type = (TransactionType)x.TransactionType,
                    TicketNumber = x.ExternalCode?.ToString() ?? "", 
                    Branch = branch,
                    Period = x.ChangePeriod,
                    Nomenclature = nomenclature.GetValueOrDefault(x.NomenclatureId ?? Guid.Empty),
                    Emploee = employers.GetValueOrDefault(x.EmloeeId ?? Guid.Empty), 
                    Price = (double)x.Price,
                    Quantuty = (double)x.Quantity,
                    Discount = (double)x.Discount
                }
            );
    }
}
