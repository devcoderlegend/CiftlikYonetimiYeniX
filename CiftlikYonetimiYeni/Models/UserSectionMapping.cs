using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class UserSectionMapping
{
    public int Id { get; set; }

    public int? SectionId { get; set; }

    public int? UserId { get; set; }

    public DateTime? UpdateTime { get; set; }

    public int? Active { get; set; }

    public virtual Section? Section { get; set; }
}
