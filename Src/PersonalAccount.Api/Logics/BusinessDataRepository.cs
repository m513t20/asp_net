using System;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.Common.Core;
using PersonalAccount.Data.Models;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Api.Logics;

/// <summary>
/// Репозиторий для финальной записи обработанных бизнес-данных в базу данных.
/// </summary>
/// // TODO
public class BusinessDataRepository : IBusinessDataRepository
{
    private readonly PersonalAccountContext _context;

    public BusinessDataRepository(PersonalAccountContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Выполняет массовое сохранение новых категорий номенклатуры.
    /// </summary>
    /// <param name="categories"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task SaveCategoriesAsync(IEnumerable<CategoryModel> categories, CancellationToken token)
    {
        if (categories == null || !categories.Any()) return;

        var entries = categories.Select( x => new Category
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }
        );

        await _context.Categories.AddRangeAsync(entries, token);
        await _context.SaveChangesAsync(token);
    }

    /// <summary>
    /// Выполняет массовое сохранение новых номенклатур.
    /// </summary>
    /// <param name="nomenclature"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task SaveNomenclatureAsync(IEnumerable<NomenclatureModel> nomenclature, CancellationToken token)
    {
        if (nomenclature == null || !nomenclature.Any()) return;

        var categoryCodes = nomenclature.Select(x => x.Category.Code).Distinct().ToList();
        var categoryMap = await _context.Categories
            .Where(x => categoryCodes.Contains((long)x.Code!))
            .ToDictionaryAsync(x => x.Code, x => x.Id, token);

        var entities = nomenclature.Select(x => new Nomenclature
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            CategoryId = categoryMap.TryGetValue(x.Category.Code, out var catId) ? catId : null 
        });

        await _context.Nomenclatures.AddRangeAsync(entities, token);
        await _context.SaveChangesAsync(token);
    }

    /// <summary>
    /// Выполняет массовое сохранение новых работников.
    /// </summary>
    /// <param name="employees"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task SaveEmployeesAsync(IEnumerable<EmploeeModel> employees, CancellationToken token)
    {
        if (employees == null || !employees.Any()) return;

        var entities = employees.Select(e => new Emploee
        {
            Id = e.Id,
            Code = e.Code,
            Name = e.Name
        });

        await _context.Emploees.AddRangeAsync(entities, token);
        await _context.SaveChangesAsync(token);
    }

    /// <summary>
    /// Выполняет массовое сохранение новых транзакций.
    /// </summary>
    /// <param name="transactions"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task SaveTransactionsAsync(IEnumerable<TransactionModel> transactions, CancellationToken token)
    {
        if (transactions == null || !transactions.Any()) return;
        
        var entities = transactions.Select(t => new Transaction
        {
            Id = Guid.NewGuid(),
            TransactionType = (int)t.Type,
            Quantity = (decimal?)t.Quantuty,
            Price = (decimal?)t.Price,
            Discount = (decimal?)t.Discount,
            ChangePeriod = t.Period.DateTime,
            BranchId = t.Owner?.Id ?? Guid.Empty,
            EmloeeId = t.Emploee?.Id,
            NomenclatureId = t.Nomenclature?.Id
        });
        await _context.Transactions.AddRangeAsync(entities, token);
        await _context.SaveChangesAsync(token);
    }
}
