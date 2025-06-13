using System;
using System.Collections.Generic;

namespace CompanyApp.Models;

public partial class Address
{
    public int Id { get; set; }

    public int CityId { get; set; }


    public string StreetName { get; set; } = null!;

    public string BuildingNumber { get; set; } = null!;

    public string? ApartmentNumber { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

}
