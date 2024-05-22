using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JeanCraftLibrary.Entity;

public partial class Address
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string? Type { get; set; }

    public string? Detail { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    [JsonIgnore]
    public virtual Account? User { get; set; }
}
