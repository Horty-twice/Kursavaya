using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class FineReason
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();
}
