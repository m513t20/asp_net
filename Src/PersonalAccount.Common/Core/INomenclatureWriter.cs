using System;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

public interface INomenclatureWriter : IWriter<NomenclatureModel, Guid>;