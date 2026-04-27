using System;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

public interface IEmployeeWriter : IWriter<EmploeeModel, Guid>;