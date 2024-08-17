using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class Department
{
    public int Id { get; set; }

    public int? CompanyId { get; set; }

    public string? DepartmentName { get; set; }

    public string? DepartmentDescription { get; set; }

    public int? Active { get; set; }

    public DateTime? UpdateTime { get; set; }

    public virtual Company? Company { get; set; }

    public virtual ICollection<DeviceDepartmentMapping> DeviceDepartmentMappings { get; set; } = new List<DeviceDepartmentMapping>();

    public virtual ICollection<Farm> Farms { get; set; } = new List<Farm>();
}
