using System;
using System.Collections.Generic;

namespace PersonalAccount.Data.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Connection> Connections { get; set; } = new List<Connection>();
}
