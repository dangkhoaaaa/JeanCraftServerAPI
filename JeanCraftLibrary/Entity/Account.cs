using System;
using System.Collections.Generic;

namespace JeanCraftLibrary.Entity;

public partial class Account
{
    public Guid UserId { get; set; }

    public string? UserName { get; set; }

    public string? Phonenumber { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Image { get; set; }

    public bool? Status { get; set; }

    public Guid? RoleId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<CartItem> Carts { get; set; } = new List<CartItem>();
}
