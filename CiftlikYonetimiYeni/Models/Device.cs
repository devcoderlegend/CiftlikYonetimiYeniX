using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class Device
{
    public int Id { get; set; }

    public string? DeviceName { get; set; }

    public string? DeviceDescription { get; set; }

    public string? Guid { get; set; }

    public string? UniqueId { get; set; }

    public DateTime? UpdateTime { get; set; }

    public int? Active { get; set; }

    public virtual ICollection<DeviceDepartmentMapping> DeviceDepartmentMappings { get; set; } = new List<DeviceDepartmentMapping>();

    public virtual ICollection<DeviceProfile> DeviceProfiles { get; set; } = new List<DeviceProfile>();
}
