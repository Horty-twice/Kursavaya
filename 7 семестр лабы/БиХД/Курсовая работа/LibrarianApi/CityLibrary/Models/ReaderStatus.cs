using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class ReaderStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Reader> Readers { get; set; } = new List<Reader>();
}
