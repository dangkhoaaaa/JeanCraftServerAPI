﻿using System;
using System.Collections.Generic;

namespace JeanCraftLibrary.Entity;

public partial class Role
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
