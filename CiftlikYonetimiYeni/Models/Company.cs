using System;
using System.Collections.Generic;

namespace CiftlikYonetimiYeni.Models;

public partial class Company
{
    public int Id { get; set; }

    public string? CompanyName { get; set; }

    public string? CompanyDescription { get; set; }

    public byte[]? Logo { get; set; }

    public string? Address { get; set; }

    public int? Active { get; set; }

    public DateTime? LastUpdate { get; set; }

    public virtual ICollection<CompanyDetail> CompanyDetails { get; set; } = new List<CompanyDetail>();

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}
