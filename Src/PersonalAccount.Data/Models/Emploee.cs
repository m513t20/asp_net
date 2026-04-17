using System;
using System.Collections.Generic;

namespace PersonalAccount.Data.Models;

public partial class Emploee
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public Guid? AffiliateId { get; set; }

    public virtual Affiliate? Affiliate { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
