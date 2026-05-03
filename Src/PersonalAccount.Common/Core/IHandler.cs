using PersonalAccount.Domain.Core;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Абстрактный интерфейс репозиторий для наследования.
/// </summary>
public interface IHandler<T> 
    where T: IDto;

