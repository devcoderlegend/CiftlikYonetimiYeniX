using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class Weight
{
    public int Id { get; set; }

    public float? Weight1 { get; set; }

    public int? DeviceValueReceiveId { get; set; }

    public byte[]? DataValue { get; set; }

    public DateTime? InsertTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public int? UserId { get; set; }

    public virtual DeviceValueReceive? DeviceValueReceive { get; set; }

    public virtual User? User { get; set; }
}
