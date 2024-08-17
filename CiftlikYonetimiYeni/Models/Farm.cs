using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class Farm
{
    public int Id { get; set; }

    public int? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Stable> Stables { get; set; } = new List<Stable>();
}
