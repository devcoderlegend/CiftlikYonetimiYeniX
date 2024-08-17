using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class DeviceProfile
{
    public int Id { get; set; }

    public int? DeviceId { get; set; }

    public int? CommunicationProtocoleId { get; set; }

    public string? StartByte { get; set; }

    public string? EndByte { get; set; }

    public int? TotalByteLength { get; set; }

    public DateTime? UpdateTime { get; set; }

    public virtual Device? Device { get; set; }

    public virtual ICollection<DeviceProfileAttribute> DeviceProfileAttributes { get; set; } = new List<DeviceProfileAttribute>();
}
