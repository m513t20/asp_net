using System;
using PersonalAccount.Domain.Models;
using PersonalAccount.Web.Interfaces;

namespace PersonalAccount.Web.Storage;

/// <summary>
/// Класс для хранения данных о настройках и филиалах.
/// </summary>
public class SettingsStorage : ISettingsStorage
{
    // TODO ... свести с актуальным бэком
    // пока просто локально храню данные
    // о ветках, настройках
    private IEnumerable<BranchModel> _branches;

    private LoadingSettingsModel _settings;

    public SettingsStorage()
    {
        var company = new CompanyModel()
        {
            Name = "Компания 1"
        };

        _branches = new List<BranchModel>() {
            new() {
                Id = Guid.NewGuid(),
                Owner = company,
                Name = "Ветка 1",
            },
            new() {
                Id = Guid.NewGuid(),
                Owner = company,
                Name = "Ветка 2",
            },
            new() {
                Id = Guid.NewGuid(),
                Owner = company,
                Name = "Ветка 1",
            }
        };
        
        _settings = new LoadingSettingsModel()
        {
            Id = Guid.NewGuid(),
            Branch = _branches.First(),
            StartPosition = 0,
            BatchSize = 100,
        };
    }

    public IEnumerable<BranchModel> Branches { 
        get {return _branches;} 
    }

    public LoadingSettingsModel Settings { 
        get {return _settings;} 
        set {_settings = value;} 
    }

    public BranchModel Branch => Settings.Branch;
}
