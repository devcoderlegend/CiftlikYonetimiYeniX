using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class DataType
{
    public int Id { get; set; }

    public string? DataTypeName { get; set; }

    public string? DataTypeDescription { get; set; }

    public DateTime? UpdateTime { get; set; }

    public int? Active { get; set; }

    public virtual ICollection<DeviceProfileAttribute> DeviceProfileAttributes { get; set; } = new List<DeviceProfileAttribute>();
}
