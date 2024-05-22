using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JeanCraftLibrary.Entity;

public partial class DesignOne
{
    public Guid DesignOneId { get; set; }

    public Guid? Fit { get; set; }

    public Guid? Length { get; set; }

    public Guid? Cuffs { get; set; }

    public Guid? Fly { get; set; }

    public Guid? FrontPocket { get; set; }

    public Guid? BackPocket { get; set; }

    public virtual Component? BackPocketNavigation { get; set; }

    public virtual Component? CuffsNavigation { get; set; }

    public virtual Component? FitNavigation { get; set; }

    public virtual Component? FlyNavigation { get; set; }

    public virtual Component? FrontPocketNavigation { get; set; }

    public virtual Component? LengthNavigation { get; set; }
    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
