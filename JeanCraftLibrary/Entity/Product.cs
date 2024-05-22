using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JeanCraftLibrary.Entity;

public partial class Product
{
    public Guid ProductId { get; set; }

    public string? Image { get; set; }

    public Guid? DesignOneId { get; set; }

    public Guid? DesignTwoId { get; set; }

    public Guid? DesignThreeId { get; set; }

    public double? Price { get; set; }

    public double? Size { get; set; }
    [JsonIgnore]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual DesignOne? DesignOne { get; set; }

    public virtual DesignThree? DesignThree { get; set; }

    public virtual DesignTwo? DesignTwo { get; set; }
    [JsonIgnore]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ProductInventory? ProductInventory { get; set; }
}
