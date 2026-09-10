using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class Library
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string BuildingNumber { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Reader> Readers { get; set; } = new List<Reader>();
}
