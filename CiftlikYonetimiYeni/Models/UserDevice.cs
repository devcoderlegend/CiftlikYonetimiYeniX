using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class UserDevice
{
    public int Id { get; set; }

    public string? UserAgent { get; set; }

    public string? DeviceId { get; set; }

    public int? Authorized { get; set; }

    public DateTime? UpdateTime { get; set; }

    public int? IsMobile { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public string? BrandName { get; set; }

    public string? Model { get; set; }

    public int? Active { get; set; }

    public int? UserDeviceTypeId { get; set; }

    public string? GeneratedKey { get; set; }

    public virtual UserDeviceType? UserDeviceType { get; set; }

    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
}
