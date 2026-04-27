using System;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Интерфейс для записи данных о журнале в бд.
/// </summary>
public interface IJournalWriter : IWriter<JournalRowDto, Guid>;