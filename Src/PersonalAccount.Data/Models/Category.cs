using System;
using System.Collections.Generic;

namespace PersonalAccount.Data.Models;

public partial class Category
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid OrganisationId { get; set; }

    public virtual ICollection<Journal> Journals { get; set; } = new List<Journal>();

    public virtual ICollection<Nomenclature> Nomenclatures { get; set; } = new List<Nomenclature>();

    public virtual Organisation Organisation { get; set; } = null!;
}
