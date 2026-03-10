using System;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Domain.Utils;

/// <summary>
/// Класс для формирования отчетов.
/// </summary>
public class ReportMaster
{
    /// <summary>
    /// Метод для получения отчетов по графику работы.
    /// </summary>
    /// <param name="transactions"></param>
    /// <returns></returns>
    public static List<DtoJobSchedule> GetJobReport(IList<Models.Transaction> transactions)
    {
        var result = new List<DtoJobSchedule>();
        var startTransactions = transactions
            .Where( x => x.Type == Models.Enums.TransactionType.JobStart)
            .OrderBy( x => x.OpeningTime )
            .ToList();
        var finishTransactions = transactions
            .Where( x => x.Type == Models.Enums.TransactionType.JobFinish)
            .OrderBy( x => x.ClosingTime )
            .ToList();

        var employers= startTransactions.Select( x => x.ServedBy );

        foreach (var employee in employers)
        {
            var employeeTransactions = startTransactions
                .Where( x => x.ServedBy.Id == employee.Id );

            foreach (var startTransaction in employeeTransactions)
            {
                var dto = new DtoJobSchedule();
                var finishTransaction = finishTransactions.First( x => x.ClosingTime.Date == startTransaction.ClosingTime.Date);

                dto.EmployeeId = employee.Id;
                dto.StartTime = startTransaction.ClosingTime;
                dto.FinishTime = finishTransaction.ClosingTime;
                dto.Name = employee.Name;
                dto.OrganisationId = employee.WorkOrganisation.Id;

                result.Add(dto);
            }
            
        }

        return result;
    }

    /// <summary>
    /// Метод для получения отчетов по выручке.
    /// </summary>
    /// <param name="transactions"></param>
    public static List<DtoProfit> GetProfitReport(IList<Transaction> transactions)
    {
        var result = new List<DtoProfit>();
        var filteredTransactions = transactions
            .Where( x => x.Type != Models.Enums.TransactionType.Cash)
            .Where( x => x.Type != Models.Enums.TransactionType.Visa)
            .Where( x => x.Type != Models.Enums.TransactionType.WriteOff)
            .ToList();
        var organisations = filteredTransactions.Select( x => x.ServedIn );

        foreach (var organisation in organisations)
        {
            var organisationTransactions = filteredTransactions.Where( x => x.ServedIn.Id == organisation.Id );
            var dto = new DtoProfit();
            var cashProfit = 0.0;
            var cardProfit = 0.0;
            var otherProfit = 0.0;
            var discount = 0.0;
            var IsHoliday = false;

            foreach (var transaction in organisationTransactions)
            {
                switch (transaction.Type)
                {
                    case Models.Enums.TransactionType.Cash:
                        cashProfit += transaction.Amount * transaction.Total;
                        break;
                    case Models.Enums.TransactionType.Visa:
                        cardProfit += transaction.Amount * transaction.Total;
                        break;
                    case Models.Enums.TransactionType.WriteOff:
                        otherProfit += transaction.Amount * transaction.Total;
                        break;
                }
                discount += transaction.Discount;
            }   

            dto.CardProfit = cardProfit;
            dto.CashProfit = cashProfit;
            dto.OtherProfit = otherProfit;
            dto.Discount = discount;
            dto.IsHoliday = IsHoliday;
            result.Add(dto);
        }

        return result;
    }

    /// <summary>
    /// Метод для получения отчетов по продажам.
    /// </summary>
    /// <param name="transactions"></param>
    public static List<DtoSales> GetSalesReport(IList<Models.Transaction> transactions)
    {
        var result = new List<DtoSales>();
        var filteredTransactions = transactions
            .Where( x => x.Type != Models.Enums.TransactionType.PLUSale)
            .ToList();
        var nomenclatures = filteredTransactions.Select( x => x.UsedNomenclature );
        var organisations = filteredTransactions.Select( x => x.ServedIn );

        foreach (var nomenclature in nomenclatures)
        {
            foreach (var organisation in organisations)
            {
                var currentTransactions = filteredTransactions
                    .Where( x => x.UsedNomenclature.Id == nomenclature.Id)
                    .Where( x => x.ServedIn.Id == organisation.Id);

                var dto = new DtoSales();
                dto.CategoryId = nomenclature.ParentCategory.Id;
                dto.CategoryName = nomenclature.ParentCategory.Name;
                dto.NomenclatureId = nomenclature.Id;
                dto.NomenclatureName = nomenclature.Name;
                var discount = 0.0;
                var summ = 0.0;

                foreach (var transaction in transactions)
                {
                    discount += transaction.Discount;
                    summ += transaction.Amount * transaction.Total;
                }
                result.Add(dto);
            }
        }

        return result;
    }
}
