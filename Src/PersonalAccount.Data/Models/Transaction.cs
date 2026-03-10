using System;
using System.Collections.Generic;

namespace PersonalAccount.Data.Models;

public partial class Transaction
{
    public long Id { get; set; }

    public long? Type { get; set; }

    public long? EmployeeId { get; set; }

    public Guid? OrganisationId { get; set; }

    public DateTime Opened { get; set; }

    public DateTime? Closed { get; set; }

    public virtual Employer? Employee { get; set; }

    public virtual ICollection<Journal> Journals { get; set; } = new List<Journal>();

    public virtual Organisation? Organisation { get; set; }

    public virtual TransactionType? TypeNavigation { get; set; }
}
