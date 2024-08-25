using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class DeviceValueReceive
{
    public int Id { get; set; }

    public int? DeviceDepartmentMappingId { get; set; }

    public byte[]? ReceivedInformation { get; set; }

    public DateTime? InsertTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public int? Active { get; set; }

    public virtual DeviceUserMapping? DeviceDepartmentMapping { get; set; }

    public virtual ICollection<Rfid> Rfids { get; set; } = new List<Rfid>();

    public virtual ICollection<Weight> Weights { get; set; } = new List<Weight>();
}
