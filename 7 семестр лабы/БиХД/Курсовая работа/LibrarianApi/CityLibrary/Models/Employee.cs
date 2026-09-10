using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public DateOnly EmploymentDate { get; set; }

    public int Positionid { get; set; }

    public int Libraryid { get; set; }

    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();

    public virtual Library Library { get; set; } = null!;

    public virtual ICollection<ManagerRequest> ManagerRequests { get; set; } = new List<ManagerRequest>();

    public virtual Position Position { get; set; } = null!;
}
