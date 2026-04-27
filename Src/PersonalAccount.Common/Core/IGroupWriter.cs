using System;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

public interface IGroupWriter : IWriter<CategoryModel, Guid>;