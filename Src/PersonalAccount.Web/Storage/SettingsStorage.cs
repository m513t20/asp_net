using PersonalAccount.Common.Core;
using PersonalAccount.Data;
using PersonalAccount.Domain.Models;
using PersonalAccount.Web.Interfaces;

namespace PersonalAccount.Web.Storage;

/// <summary>
/// Класс для хранения данных о настройках и филиалах.
/// </summary>
public class SettingsStorage(
        ICompanySettingsRepository settingsRepository,
        PersonalAccountContext context
    ) : ISettingsStorage
{
    /// <summary>
    /// Репозиторий для работы с настройками.
    /// </summary>
    private readonly ICompanySettingsRepository _settingReposity = settingsRepository;

    /// <summary>
    /// Репозиторий для работы с настройками.
    /// </summary>
    private readonly PersonalAccountContext _context = context;

    /// <summary>
    /// Получить Филиалы.
    /// </summary>
    public IEnumerable<BranchModel> Branches => _context.Branches.Select(x =>
                new BranchModel()
                {
                    Id = x.Id,
                    Name = x.Name
                }
            );

    /// <summary>
    /// Получить настройки по филиалу.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public LoadingSettingsModel GetSettingsByBranchId(Guid id)
    {
        var branch = new BranchModel() { Id = id };
        return _settingReposity.Load(branch) ?? throw new ArgumentNullException($"Настройки по Id={id} не найдены");
    }

    /// <summary>
    /// Сохранить настройки.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public bool SaveLoadingSettings(LoadingSettingsModel model)
    {
        _settingReposity.Save(model);
        return true;
    }
}
