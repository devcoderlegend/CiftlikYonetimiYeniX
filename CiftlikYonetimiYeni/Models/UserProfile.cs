using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class UserProfile
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? FullName { get; set; }

    public string? NationalId { get; set; }

    public byte[]? Photo { get; set; }

    public string? StaffId { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public int? Active { get; set; }

    public virtual User? User { get; set; }
}
