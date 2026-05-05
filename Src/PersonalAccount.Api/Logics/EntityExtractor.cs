using System;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.Common.Core;
using PersonalAccount.Data.Models;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Api.Logics;

/// <summary>
/// Сервис для выделения новых бизнес-сущностей из потока сырых данных.
/// </summary>
public class EntityExtractor : IEntityExtractor
{
    private readonly PersonalAccountContext _context;

    public EntityExtractor(PersonalAccountContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получить новые категории.
    /// </summary>
    /// <param name="rows"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<IEnumerable<CategoryModel>> ExtractNewCategoriesAsync(IEnumerable<JournalRowDto> rows, CancellationToken token)
    {
        var newCategories = rows 
            .Where( x => x.CategoryCode.HasValue && ! string.IsNullOrWhiteSpace(x.CategoryName) )
            .GroupBy( x => x.CategoryCode!.Value)
            .Select( x => x.First())
            .ToList();
        
        if (newCategories.Count == 0) return Enumerable.Empty<CategoryModel>();
        
        var newIds = newCategories.Select( x=>
            x.CategoryCode!.Value
        ).ToList();

        var existingCodes = await _context.Categories
            .Where(x => newIds.Contains((long)x.Code!))
            .Select(x => x.Code)
            .ToListAsync(token);

        return newCategories
            .Where(x => !existingCodes.Contains(x.CategoryCode!.Value))
            .Select(x => new CategoryModel
            {
                Id = Guid.NewGuid(),
                Code = x.CategoryCode!.Value,
                Name = x.CategoryName!,
            });
    }   

    /// <summary>
    /// Получить новую номенклатуру.
    /// </summary>
    /// <param name="rows"></param>
    /// <param name="token"></param>
    public async Task<IEnumerable<NomenclatureModel>> ExtractNewNomenclatureAsync(IEnumerable<JournalRowDto> rows, CancellationToken token)
    {
        var newNomenclature = rows
            .Where(x => x.ProductCode.HasValue && !string.IsNullOrWhiteSpace(x.ProductName))
            .GroupBy(x => x.ProductCode!.Value)
            .Select(x => x.First())
            .ToList();

        if (!newNomenclature.Any()) return Enumerable.Empty<NomenclatureModel>();

        var newIds = newNomenclature.Select(x => x.ProductCode!.Value).ToList();

        var existingCodes = await _context.Nomenclatures
            .Where(x => newIds.Contains((long)x.Code!))
            .Select(x => x.Code)
            .ToListAsync(token);

        return newNomenclature
            .Where(x => !existingCodes.Contains(x.ProductCode!.Value))
            .Select(x => new NomenclatureModel
            {
                Id = Guid.NewGuid(),
                Code = x.ProductCode!.Value,
                Name = x.ProductName!,

            });
    }

    /// <summary>
    /// Получить новых работников.
    /// </summary>
    /// <param name="rows"></param>
    /// <param name="token"></param>
    public async Task<IEnumerable<EmploeeModel>> ExtractNewEmployeesAsync(IEnumerable<JournalRowDto> rows, CancellationToken token)
    {
        var newEmployees = rows
            .Where(x => x.EmploeeCode.HasValue && !string.IsNullOrWhiteSpace(x.EmploeeName))
            .GroupBy(x => x.EmploeeCode!.Value)
            .Select(x => x.First())
            .ToList();

        if (!newEmployees.Any()) return Enumerable.Empty<EmploeeModel>();

        var newIds = newEmployees.Select(x => x.EmploeeCode!.Value).ToList();

        var existingCodes = await _context.Emploees
            .Where(x => newIds.Contains((long)x.Code!))
            .Select(x => x.Code)
            .ToListAsync(token);

        return newEmployees
            .Where(x => !existingCodes.Contains(x.EmploeeCode!.Value))
            .Select(x => new EmploeeModel
            {
                Id = Guid.NewGuid(),
                Code = x.EmploeeCode!.Value,
                Name = x.EmploeeName!
            });
    }
}
