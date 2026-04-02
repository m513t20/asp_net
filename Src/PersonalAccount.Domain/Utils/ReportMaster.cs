using System;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;
using PersonalAccount.Domain.Models.Enums;

namespace PersonalAccount.Domain.Utils;

/// <summary>
/// Класс для формирования отчетов.
/// </summary>
public class ReportMaster
{
    private record WorkScheduleKey (DateTime Period, Employee Emploee);

    /// <summary>
    /// Метод для получения отчетов по графику работы.
    /// </summary>
    /// <param name="transactions"></param>
    /// <returns></returns>
    public static IEnumerable<DtoJobSchedule> GetJobReport(IList<Models.Transaction> transactions)
    {
        if(!transactions.Any()) return  Enumerable.Empty<DtoJobSchedule>() ;
        var companyId = transactions.FirstOrDefault()?.ServedIn.Id ?? throw new InvalidOperationException("Невозможно получить код корганизации!");

        // Получаем все старты в разрезе каждого дня
        var starting =  Task.Run( () => 
                         transactions
                        .Where(x => x.Type == TransactionType.JobStart)
                        .GroupBy(
                            x => new WorkScheduleKey(x.ClosingTime.Date, x.ServedBy),
                            x => x,
                            (key, group) => new
                            {
                                Key = key,
                                Transactions = group.ToList()
                            })
                            .ToDictionary(x => x.Key, x => x.Transactions));

        // Получаем все окончания 
        var stopping = Task.Run( () => 
                         transactions
                        .Where(x => x.Type == TransactionType.JobFinish)
                        .GroupBy(
                            x => new WorkScheduleKey(x.ClosingTime.Date, x.ServedBy),
                            x => x,
                            (key, group) => new
                            {
                                Key = key,
                                Transactions = group.ToList()
                            })
                            .ToDictionary(x => x.Key, x => x.Transactions));


        // Все сотрудники
        var emploees = Task.Run( () =>
                     transactions
                    .Where(x => x.Type == TransactionType.JobStart || x.Type == TransactionType.JobFinish)
                    .GroupBy(x => x.ServedBy)
                    .Select(x => x.Key));    

        // Все периоды
        var periods = Task.Run( () =>             
                     transactions.Where(x => x.Type == TransactionType.JobStart || x.Type == TransactionType.JobFinish)
                    .GroupBy(x => x.ClosingTime.Date)
                    .Select(x => x.Key));    


        // Ожидаем расчета
        Task.WaitAll( starting, stopping, emploees, periods);       

        // Объединяем записи
        var items = emploees.Result.SelectMany(employee =>
                        periods.Result.Select(period => new { Employee = employee, Period = period }));

        // Формируем результат
        var result = items.Select( x => new DtoJobSchedule()
        {
            EmployeeId = x.Employee.Id,
            Name = x.Employee.Name,
            StartTime = starting.Result.TryGetValue(new WorkScheduleKey(x.Period.Date, x.Employee), out var startingKey)
                   // Берем минимальное значение даты
                   ? startingKey.Min(t => t.ClosingTime).Date
                   // Или начало дня
                   : x.Period,

            FinishTime = stopping.Result.TryGetValue( new WorkScheduleKey(x.Period.Date, x.Employee), out var stoppingKey)
                    // Берем максимальное значение даты
                    ? stoppingKey.Max(t => t.ClosingTime).Date
                    // Или окончание дня
                    : x.Period.AddDays(1).AddSeconds(-1),

            OrganisationId = companyId
        });
        return result;
    }

    /// <summary>
    /// Метод для получения отчетов по выручке.
    /// </summary>
    /// <param name="transactions"></param>
    public static IEnumerable<DtoProfit> GetProfitReport(IList<Transaction> transactions)
    {
        if(!transactions.Any()) return  Enumerable.Empty<DtoProfit>() ;

        // Все скидки
        var calcDiscountTask =  Task.Run( () =>
                                transactions
                                .GroupBy(x => x.ClosingTime.Date)
                                .Select(x => new {
                                    Key  = x.Key,
                                    Value = x.Sum(t => t.Discount)
                                })
                                .ToDictionary(x => x.Key, x => x.Value));

        // Рассчитать все банковские оплаты
        var calcBankTask = Task.Run( () =>
        {
            var allDiscounts = transactions
                            .Where(x => x.Type == TransactionType.Visa)
                            .GroupBy(x => x.ClosingTime.Date)
                            .Select(x => new {
                                Key  = x.Key,
                                Value = x.Sum(t => t.Discount)
                            })
                            .ToDictionary(x => x.Key, x => x.Value);

            var allPayments = transactions
                            .Where(x => x.Type == TransactionType.Visa)
                            .GroupBy(x => x.ClosingTime.Date)
                            .Select(x => new {
                                Key  = x.Key,
                                Value = x.Sum(t => t.Total * t.Amount)
                            })
                            .ToDictionary(x => x.Key, x => x.Value);

            return allPayments.ToDictionary(
                pair => pair.Key,
                pair => pair.Value
                        - (allDiscounts.ContainsKey(pair.Key) ?  allDiscounts[ pair.Key ] : 0)
            );
        });

        // Рассчитать все оплаты наличными
        var calcCashTask = Task.Run( () =>
        {
            var allDiscounts = transactions
                            .Where(x => x.Type == TransactionType.Cash)
                            .GroupBy(x => x.ClosingTime.Date)
                            .Select(x => new {
                                Key  = x.Key,
                                Value = x.Sum(t => t.Discount)
                            })
                            .ToDictionary(x => x.Key, x => x.Value);

            var allPayments = transactions
                            .Where(x => x.Type == TransactionType.Cash)
                            .GroupBy(x => x.ClosingTime.Date)
                            .Select(x => new {
                                Key  = x.Key,
                                Value = x.Sum(t => t.Total * t.Amount)
                            })
                            .ToDictionary(x => x.Key, x => x.Value);


            var allRefunds = transactions
                            .Where(x => x.Type == TransactionType.RefundPayment)
                            .GroupBy(x => x.ClosingTime.Date)
                            .Select(x => new {
                                Key  = x.Key,
                                Value = x.Sum(t => t.Total * t.Amount)
                            })
                            .ToDictionary(x => x.Key, x => x.Value);

            return allPayments.ToDictionary(
                pair => pair.Key,
                pair => pair.Value
                        - (allDiscounts.ContainsKey(pair.Key) ?  allDiscounts[pair.Key]  : 0)
                        - (allRefunds.ContainsKey(pair.Key) ? allRefunds[pair.Key] : 0)
            );
        });

        // Ожидаем расчета
        Task.WaitAll( calcBankTask, calcCashTask, calcDiscountTask);

        // Получим список всех дат
        var periods = calcBankTask.Result.Keys
                    .Union( calcCashTask.Result.Keys )
                    .Union( calcDiscountTask.Result.Keys )
                    .Distinct()
                    .ToList();

        // Формируем результат
        var result = periods.Select( x => new DtoProfit()
        {
            StartDate = x,
            EndDate = x,
            CardProfit = calcBankTask.Result.ContainsKey( x ) ? calcBankTask.Result[ x ] : 0,
            CashProfit = calcCashTask.Result.ContainsKey( x ) ? calcCashTask.Result[ x ] : 0,
            Discount = calcDiscountTask.Result.ContainsKey( x ) ? calcDiscountTask.Result[ x ] : 0,
            OrganisationId = transactions.FirstOrDefault()?.ServedIn.Id ?? Guid.Empty
        }).OrderBy(x => x.StartDate);

        return result ;
    }

    /// <summary>
    /// Метод для получения отчетов по продажам.
    /// </summary>
    /// <param name="transactions"></param>
    public static IEnumerable<DtoSales> GetSalesReport(IList<Models.Transaction> transactions)
    {
        var filteredTransactions = transactions
            .Where( x => x.Type != Models.Enums.TransactionType.PLUSale)
            .ToList();

        var result = filteredTransactions.Select( x => new DtoSales()
        {
            CategoryId = x.UsedNomenclature.ParentCategory.Id,
            CategoryName = x.UsedNomenclature.ParentCategory.Name,
            NomenclatureId = x.UsedNomenclature.Id,
            NomenclatureName = x.UsedNomenclature.Name,
            SaleDate = x.ClosingTime,
            Amount = x.Amount,
            Cost = x.Total,
            Discount = x.Discount,
            OrganisationId = x.ServedIn.Id
        }).OrderBy( x => x.SaleDate);

        return result;
    }
}
