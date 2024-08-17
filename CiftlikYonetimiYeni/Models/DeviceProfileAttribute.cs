using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class DeviceProfileAttribute
{
    public int Id { get; set; }

    public int? DeviceProfileId { get; set; }

    public int? DataTypeId { get; set; }

    public int? StartByteIndex { get; set; }

    public int? EndByteIndex { get; set; }

    public DateTime? UpdateTime { get; set; }

    public int? Active { get; set; }

    public virtual DataType? DataType { get; set; }

    public virtual DeviceProfile? DeviceProfile { get; set; }
}
