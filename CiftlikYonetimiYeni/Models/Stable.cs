using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class Stable
{
    public int Id { get; set; }

    public int FarmId { get; set; }

    public string? FarmName { get; set; }

    public string? FarmDescription { get; set; }

    public DateTime? UpdateTime { get; set; }

    public virtual Farm Farm { get; set; } = null!;
}
