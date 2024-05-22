using System;
using System.Collections.Generic;

namespace JeanCraftLibrary.Entity;

public partial class OrderDetail
{
    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public double? PriceUnit { get; set; }

    public int? Quantity { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
