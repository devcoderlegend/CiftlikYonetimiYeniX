using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class UserSession
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? DeviceId { get; set; }

    public DateTime? LoginTime { get; set; }

    public string? GeneratedKey { get; set; }

    public DateTime? ExpireTime { get; set; }

    public DateTime? Updatetime { get; set; }

    public int? Active { get; set; }

    public string? IpAddress { get; set; }

    public virtual UserDevice? Device { get; set; }

    public virtual User? User { get; set; }
}
