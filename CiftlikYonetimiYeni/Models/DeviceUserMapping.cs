using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class DeviceUserMapping
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? DeviceId { get; set; }

    public DateTime? UpdateTime { get; set; }

    public int? Active { get; set; }

    public virtual Device? Device { get; set; }

    public virtual ICollection<DeviceValueReceive> DeviceValueReceives { get; set; } = new List<DeviceValueReceive>();

    public virtual User? User { get; set; }
}
