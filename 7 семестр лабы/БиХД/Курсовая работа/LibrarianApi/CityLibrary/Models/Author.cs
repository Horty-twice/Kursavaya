using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class Author
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
