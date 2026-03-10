using System;
using System.Collections.Generic;

namespace PersonalAccount.Data.Models;

public partial class Nomenclature
{
    public long Id { get; set; }

    public long CategoryId { get; set; }

    public string MeasureUnit { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Journal> Journals { get; set; } = new List<Journal>();
}
