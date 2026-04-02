using System;
using System.Collections.Generic;

namespace PersonalAccount.Data.Models;

public partial class LoadSetting
{
    public Guid Id { get; set; }

    public long PackSize { get; set; }

    public long LoadDataType { get; set; }

    public Guid OrganisationId { get; set; }

    public virtual LoadType LoadDataTypeNavigation { get; set; } = null!;

    public virtual Organisation Organisation { get; set; } = null!;
}
