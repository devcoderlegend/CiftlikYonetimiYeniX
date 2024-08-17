using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class DeviceDepartmentMapping
{
    public int Id { get; set; }

    public int? DepartmentId { get; set; }

    public int? DeviceId { get; set; }

    public DateTime? UpdateTime { get; set; }

    public int? Active { get; set; }

    public virtual Department? Department { get; set; }

    public virtual Device? Device { get; set; }

    public virtual ICollection<DeviceValueReceive> DeviceValueReceives { get; set; } = new List<DeviceValueReceive>();
}
