using System;
using System.Collections.Generic;

namespace JeanCraftLibrary.Entity;

public partial class CartItem
{
    public Guid? ProductId { get; set; }

    public Guid Id { get; set; }

    public int? Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<Account> Users { get; set; } = new List<Account>();
}
