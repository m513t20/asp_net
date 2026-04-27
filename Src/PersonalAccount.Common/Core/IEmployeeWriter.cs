using System;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Интерфейс для записи данных о сотрудниках в бд.
/// </summary>
public interface IEmployeeWriter : IWriter<EmploeeModel, Guid>;