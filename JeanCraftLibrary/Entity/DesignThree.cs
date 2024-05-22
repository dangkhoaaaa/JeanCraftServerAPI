using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JeanCraftLibrary.Entity;

public partial class DesignThree
{
    public Guid DesignThreeId { get; set; }

    public Guid? MonoGram { get; set; }

    public string? Characters { get; set; }

    public Guid? EmbroideryFont { get; set; }

    public Guid? EmbroideryColor { get; set; }

    public Guid? StitchingThreadColor { get; set; }

    public Guid? ButtonAndRivet { get; set; }

    public Guid? BranchBackPatch { get; set; }

    public virtual Component? BranchBackPatchNavigation { get; set; }

    public virtual Component? ButtonAndRivetNavigation { get; set; }

    public virtual Component? EmbroideryColorNavigation { get; set; }

    public virtual Component? EmbroideryFontNavigation { get; set; }

    public virtual Component? MonoGramNavigation { get; set; }
    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual Component? StitchingThreadColorNavigation { get; set; }
}
