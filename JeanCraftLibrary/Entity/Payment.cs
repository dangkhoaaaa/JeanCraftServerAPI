using System;
using System.Collections.Generic;

namespace JeanCraftLibrary.Entity;

public partial class Payment
{
    public Guid Id { get; set; }

    public Guid? OrderId { get; set; }

    public double? Amount { get; set; }

    public string? Method { get; set; }

    public string? Status { get; set; }

    public virtual Order? Order { get; set; }
}
