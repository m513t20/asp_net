using System;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Common.Core;

public interface IJournalWriter : IWriter<JournalRowDto, Guid>;