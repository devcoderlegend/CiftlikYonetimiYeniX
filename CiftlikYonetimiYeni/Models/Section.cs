using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class Section
{
    public int Id { get; set; }

    public int? StableId { get; set; }

    public string? SectionName { get; set; }

    public string? SectionDescription { get; set; }

    public DateTime? UpdateTime { get; set; }

    public int? Active { get; set; }

    public virtual ICollection<UserSectionMapping> UserSectionMappings { get; set; } = new List<UserSectionMapping>();
}
