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
    /// Получить модель филиала
    /// </summary>
    /// <param name="id"> Уникальный код </param>
    /// <returns></returns>
    public BranchModel GetBranch(Guid id);

    /// <summary>
    /// Ассинхронный вариант получение филиала
    /// </summary>
    /// <param name="id"> Уникальный код </param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<BranchModel> GetBranchAsync(Guid id, CancellationToken token);

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
