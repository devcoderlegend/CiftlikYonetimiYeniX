using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class User
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public DateTime? UpdateTime { get; set; }

    public int? Active { get; set; }

    public virtual ICollection<DeviceUserMapping> DeviceUserMappings { get; set; } = new List<DeviceUserMapping>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<Rfid> Rfids { get; set; } = new List<Rfid>();

    public virtual ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();

    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();

    public virtual ICollection<Weight> Weights { get; set; } = new List<Weight>();
}
