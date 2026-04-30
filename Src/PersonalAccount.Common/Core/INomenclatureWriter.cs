using System;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Интерфейс для записи данных о номенклатуре в бд.
/// </summary>
public interface INomenclatureWriter : IWriter<NomenclatureModel, Guid>;