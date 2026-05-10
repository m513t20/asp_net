using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.Common.Core;
using PersonalAccount.Data;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Web.Logics;

/// <summary>
/// Реализация интерфейса <see cref="IBranchRepository"/>
/// </summary>
public class BranchRepository(PersonalAccountContext context, ISettingsRepository settingsRepostory) : IBranchRepository
{   
    // Контекст для работы с базой данных
    private readonly PersonalAccountContext _context = context;

    // Репозиторий для работы с настройками
    private readonly ISettingsRepository _settingsRepostory = settingsRepostory;

    /// <inhericdoc/>
    public IEnumerable<BranchModel> GetBranches()
    {
        var branches = _context.Branches
            .Include(x => x.Company)
            .Select( x => new BranchModel()
            {
                Id = x.Id,
                Name = x.Name ?? "Без названия",
                Owner = new CompanyModel()
                {
                    Id = x.Company.Id,
                    Address = x.Company.Address ?? string.Empty,
                    INN = x.Company.Inn ?? string.Empty,
                    Name = x.Company.Name ?? "Без названия"
                }
            }).ToList();

        foreach(var branch in branches)
        {
            var settings = _settingsRepostory.Load( branch ) ?? throw new InvalidOperationException($"Невозможно получить настройки для филиала {branch.Id}");
            branch.Settings = settings;
        }    

        return branches;
    }

    /// <inhericdoc/>
    public async Task<IEnumerable<BranchModel>> GetBranchesAsync(CancellationToken token)
        => await Task.Run( () => GetBranches(), token);

    /// <inhericdoc/>
    public void Update(BranchModel model)
    {
        var isValid = model.Validate();
        if(!isValid) throw new InvalidOperationException($"Невозможно обновить данные!\n{model.ErrorText}");

        var entity = _context.Branches.Find( model.Id );
        if(entity is null) throw new InvalidOperationException($"Невозможно найти филиал с кодом {model.Id}!");

        entity.Name = model.Name;
        entity.LoadOptions = JsonSerializer.Serialize(  model.Settings );
        _context.SaveChanges();
    }

    /// <inhericdoc/>
    public async Task UpdateAsync(BranchModel model, CancellationToken token)
        => await Task.Run( () => Update(model), token);
}
