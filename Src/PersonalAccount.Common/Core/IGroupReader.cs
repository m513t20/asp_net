using System;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Интерфейс для чтения групп номенклатуры с клиента.
/// </summary>
public interface IGroupReader : IClientRepository<CategoryModel>;