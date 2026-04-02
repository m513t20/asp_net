using System;
using System.Collections.Generic;

namespace PersonalAccount.Data.Models;

public partial class Journal
{
    public int Id { get; set; }

    public string RecieptNumber { get; set; } = null!;

    public long? EmployeeId { get; set; }

    public long NomenclatureId { get; set; }

    public string Description { get; set; } = null!;

    public long CategoryId { get; set; }

    public long TransactionId { get; set; }

    public DateTime TransactionDate { get; set; }

    public long Amount { get; set; }

    public long Total { get; set; }

    public long Discount { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Employer? Employee { get; set; }

    public virtual Nomenclature Nomenclature { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;
}
