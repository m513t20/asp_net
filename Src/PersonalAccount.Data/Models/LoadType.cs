using System;
using System.Collections.Generic;

namespace PersonalAccount.Data.Models;

public partial class LoadType
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<LoadSetting> LoadSettings { get; set; } = new List<LoadSetting>();
}
