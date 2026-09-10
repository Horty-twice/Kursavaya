using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class SupplyItemStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<SupplyItem> SupplyItems { get; set; } = new List<SupplyItem>();
}
