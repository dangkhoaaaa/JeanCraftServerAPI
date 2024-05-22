using System;
using System.Collections.Generic;

namespace JeanCraftLibrary.Entity;

public partial class ShoppingCart
{
    public Guid CartId { get; set; }

    public Guid UserId { get; set; }
}
