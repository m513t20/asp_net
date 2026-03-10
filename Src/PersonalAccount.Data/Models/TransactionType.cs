using System;
using System.Collections.Generic;

namespace PersonalAccount.Data.Models;

public partial class TransactionType
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
