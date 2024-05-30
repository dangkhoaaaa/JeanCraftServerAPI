using System;
using System.Collections.Generic;

namespace JeanCraftLibrary.Entity;

public partial class ShoppingCart
{
    public Guid CartId { get; set; }

    public Guid UserId { get; set; }

    public virtual CartItem Cart { get; set; } = null!;

    public virtual Account User { get; set; } = null!;
}
