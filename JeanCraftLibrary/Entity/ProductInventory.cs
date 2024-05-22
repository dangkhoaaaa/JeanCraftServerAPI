using System;
using System.Collections.Generic;

namespace JeanCraftLibrary.Entity;

public partial class ProductInventory
{
    public Guid ProductInventoryId { get; set; }

    public int? Quantity { get; set; }

    public virtual Product ProductInventoryNavigation { get; set; } = null!;
}
