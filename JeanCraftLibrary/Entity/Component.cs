using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JeanCraftLibrary.Entity;

public partial class Component
{
    public Guid ComponentId { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public double? Prize { get; set; }

    public Guid? Type { get; set; }
    [JsonIgnore]
    public virtual ICollection<DesignOne> DesignOneBackPocketNavigations { get; set; } = new List<DesignOne>();
    [JsonIgnore]
    public virtual ICollection<DesignOne> DesignOneCuffsNavigations { get; set; } = new List<DesignOne>();
    [JsonIgnore]
    public virtual ICollection<DesignOne> DesignOneFitNavigations { get; set; } = new List<DesignOne>();
    [JsonIgnore]
    public virtual ICollection<DesignOne> DesignOneFlyNavigations { get; set; } = new List<DesignOne>();
    [JsonIgnore]
    public virtual ICollection<DesignOne> DesignOneFrontPocketNavigations { get; set; } = new List<DesignOne>();
    [JsonIgnore]
    public virtual ICollection<DesignOne> DesignOneLengthNavigations { get; set; } = new List<DesignOne>();
    [JsonIgnore]
    public virtual ICollection<DesignThree> DesignThreeBranchBackPatchNavigations { get; set; } = new List<DesignThree>();
    [JsonIgnore]
    public virtual ICollection<DesignThree> DesignThreeButtonAndRivetNavigations { get; set; } = new List<DesignThree>();
    [JsonIgnore]
    public virtual ICollection<DesignThree> DesignThreeEmbroideryColorNavigations { get; set; } = new List<DesignThree>();
    [JsonIgnore]
    public virtual ICollection<DesignThree> DesignThreeEmbroideryFontNavigations { get; set; } = new List<DesignThree>();
    [JsonIgnore]
    public virtual ICollection<DesignThree> DesignThreeMonoGramNavigations { get; set; } = new List<DesignThree>();
    [JsonIgnore]
    public virtual ICollection<DesignThree> DesignThreeStitchingThreadColorNavigations { get; set; } = new List<DesignThree>();
    [JsonIgnore]
    public virtual ICollection<DesignTwo> DesignTwoFabricColorNavigations { get; set; } = new List<DesignTwo>();
    [JsonIgnore]
    public virtual ICollection<DesignTwo> DesignTwoFinishingNavigations { get; set; } = new List<DesignTwo>();

    public virtual ComponentType? TypeNavigation { get; set; }
}
