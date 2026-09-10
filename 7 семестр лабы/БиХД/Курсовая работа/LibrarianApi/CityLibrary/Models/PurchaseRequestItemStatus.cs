using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class PurchaseRequestItemStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<PurchaseRequestItem> PurchaseRequestItems { get; set; } = new List<PurchaseRequestItem>();
}
