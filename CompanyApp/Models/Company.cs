using System;
using System.Collections.Generic;

namespace CompanyApp.Models;

public partial class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public int? AddressId { get; set; }

    public virtual Address? Address { get; set; }
}
