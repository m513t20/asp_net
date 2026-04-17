using System;
using System.Collections.Generic;

namespace PersonalAccount.Data.Models;

public partial class Affiliate
{
    public Guid Id { get; set; }

    public Guid? CompanyId { get; set; }

    public string? Inn { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? LoadOptions { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual Company? Company { get; set; }

    public virtual ICollection<Emploee> Emploees { get; set; } = new List<Emploee>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
