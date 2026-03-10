using System;
using System.Collections.Generic;

namespace PersonalAccount.Data.Models;

public partial class Connection
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid OrganisationId { get; set; }

    public virtual Organisation Organisation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
