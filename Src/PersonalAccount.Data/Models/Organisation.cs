using System;
using System.Collections.Generic;

namespace PersonalAccount.Data.Models;

public partial class Organisation
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Adress { get; set; } = null!;

    public string? LoadOptions { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Connection> Connections { get; set; } = new List<Connection>();

    public virtual ICollection<Employer> Employers { get; set; } = new List<Employer>();

    public virtual ICollection<LoadSetting> LoadSettings { get; set; } = new List<LoadSetting>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
