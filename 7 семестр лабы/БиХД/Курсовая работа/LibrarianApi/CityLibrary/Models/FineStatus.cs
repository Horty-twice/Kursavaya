using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class FineStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();
}
