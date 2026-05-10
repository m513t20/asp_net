using System;
using System.ComponentModel;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Репозиторий для работы с фииалами
/// </summary>
public interface IBranchRepository
{
    /// <summary>
    /// Получить текущий список филиалов.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<BranchModel> GetBranches();

    /// <summary>
    /// Ассннхонный вариант получения списка
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<IEnumerable<BranchModel>> GetBranchesAsync(CancellationToken token);

    /// <summary>
    /// Изменить параметры филиала
    /// </summary>
    /// <param name="model"></param>
    public void Update(BranchModel model);

    /// <summary>
    /// Ассинхронный вариант изменение параметров филиала
    /// </summary>
    /// <param name="model"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task UpdateAsync(BranchModel model, CancellationToken token);
}
