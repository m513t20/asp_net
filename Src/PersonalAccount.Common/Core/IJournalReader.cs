using System;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Интерфейс для чтения журнала с клиента.
/// </summary>
public interface IJournalReader : IClientRepository<JournalRowDto>;