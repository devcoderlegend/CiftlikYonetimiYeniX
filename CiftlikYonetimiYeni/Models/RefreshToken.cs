using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class RefreshToken
{
    public int Id { get; set; }

    public string? Token { get; set; }

    public DateTime? Expires { get; set; }

    public int? IsExpired { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Revoked { get; set; }

    public int? IsActive { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
