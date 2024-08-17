using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class CompanyDetail
{
    public int Id { get; set; }

    public int? CompanyId { get; set; }

    public int? SubscriptionId { get; set; }

    public int? Active { get; set; }

    public DateTime? UpdateTime { get; set; }

    public virtual Company? Company { get; set; }
}
