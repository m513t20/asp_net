using System;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Интерфейс для чтения ноиенклатуры с клиента.
/// </summary>
public interface INomenclatureReader : IClientRepository<NomenclatureModel>;