using System.Text.Json;
using PersonalAccount.Common.Core;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Data.Logics;

/// <summary>
/// Реализация интерфейса <see cref="ICompanySettingsRepository"/>
/// </summary>
public class CompanySettingsRepository : ICompanySettingsRepository
{
    // Контекст для работы с базой данных
    private readonly PersonalAccountContext _context;

    /// <summary>
    /// Создать обхект типа <see cref="CompanySettingsRepository"/>
    /// </summary>
    /// <param name="context"> Контекст для работы с базой данных </param>
    public CompanySettingsRepository(PersonalAccountContext context) => _context = context;

    /// <summary>
    /// Загрузить настройки
    /// </summary>
    /// <param name="branch"> Филиал. </param>
    /// <returns></returns>
    public LoadingSettingsModel? Load(BranchModel branch)
    {
        var item = _context.Branches.FirstOrDefault(x => x.Id == branch.Id)
            ?? throw new InvalidDataException($"Не найдена организация по коду {branch.Id}!");

        if(string.IsNullOrEmpty( item.LoadOptions))  return null;  

        var result = JsonSerializer.Deserialize< LoadingSettingsModel>(item.LoadOptions)
            ?? throw new InvalidDataException($"Филиал по коду {branch.Id} содержит некорретные данные по настройкам!");
        return result;
    }


    /// <summary>
    /// Загрузить настройки
    /// </summary>
    /// <param name="branch"> Филиал </param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<LoadingSettingsModel?> LoadAsync(BranchModel branch, CancellationToken token)
        => await Task.Run( () => Load( branch ), token);

    /// <summary>
    /// Сохранить настройки
    /// </summary>
    /// <param name="setting"> Настройки </param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task SaveAsync(LoadingSettingsModel setting, CancellationToken token)
        => await Task.Run( () => Save(setting), token);

    /// <summary>
    /// Сохранить настройки
    /// </summary>
    /// <param name="setting"></param>
    public void Save(LoadingSettingsModel setting)
    {
        var branchId = setting.Branch?.Id ?? throw new InvalidDataException("Невозможно сохранить настройки т.к. нет информации о филиале!");
        var company = _context.Branches.FirstOrDefault(x => x.Id == branchId)
            ?? throw new InvalidDataException($"Не найден филиал по коду {branchId}!");

        var text =     JsonSerializer.Serialize(setting);
        company.LoadOptions = text;
        _context.SaveChanges();
    }
}
