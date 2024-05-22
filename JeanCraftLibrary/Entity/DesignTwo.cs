using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JeanCraftLibrary.Entity;

public partial class DesignTwo
{
    public Guid DesignTwoId { get; set; }

    public Guid? Finishing { get; set; }

    public Guid? FabricColor { get; set; }

    public virtual Component? FabricColorNavigation { get; set; }

    public virtual Component? FinishingNavigation { get; set; }
    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
