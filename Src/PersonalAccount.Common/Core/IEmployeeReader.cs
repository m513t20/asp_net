using System;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Интерфейс для чтения сотрудников с клиента.
/// </summary>
public interface IEmployeeReader : IClientRepository<EmploeeModel>;