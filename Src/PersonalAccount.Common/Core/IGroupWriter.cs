using System;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Интерфейс для записи данных о группах номенклатуры в бд.
/// </summary>
public interface IGroupWriter : IWriter<CategoryModel, Guid>;