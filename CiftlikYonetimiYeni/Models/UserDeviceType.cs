using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class UserDeviceType
{
    public int Id { get; set; }

    public string? DeviceTypeName { get; set; }

    public DateTime? UpdateTime { get; set; }

    public string? Active { get; set; }

    public virtual ICollection<UserDevice> UserDevices { get; set; } = new List<UserDevice>();
}
