using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class Reader
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string Phone { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateOnly RegistrationDate { get; set; }

    public int ReaderStatusid { get; set; }

    public int Libraryid { get; set; }

    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();

    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();

    public virtual Library Library { get; set; } = null!;

    public virtual ReaderStatus ReaderStatus { get; set; } = null!;
}
