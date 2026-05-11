using System;
using PersonalAccount.Domain.Models;
using PersonalAccount.Web.Interfaces;

namespace PersonalAccount.Web.Storage;

/// <summary>
/// Класс для хранения данных о настройках и филиалах.
/// </summary>
public class SettingsStorage : ISettingsStorage
{
    public IEnumerable<BranchModel> Branches => throw new NotImplementedException();

    public LoadingSettingsModel Settings => throw new NotImplementedException();

    public BranchModel Branch => throw new NotImplementedException();
}
