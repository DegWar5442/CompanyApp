using System;
using System.Collections.Generic;

namespace CompanyApp.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string Phone { get; set; }

    public DateOnly BirthDate { get; set; }

    public DateOnly HireDate { get; set; }

    public decimal Salary { get; set; }

    public int DepartmentId { get; set; }

    public int PositionId { get; set; }

    public int AddressId { get; set; }

    public virtual Address Address { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Position Position { get; set; } = null!;
    
}
