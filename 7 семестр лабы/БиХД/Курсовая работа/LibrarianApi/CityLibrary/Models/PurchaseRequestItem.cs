using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class PurchaseRequestItem
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public int PurchaseRequestItemStatusid { get; set; }

    public int PurchaseRequestid { get; set; }

    public int Bookid { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual PurchaseRequest PurchaseRequest { get; set; } = null!;

    public virtual PurchaseRequestItemStatus PurchaseRequestItemStatus { get; set; } = null!;

    public virtual ICollection<SupplyItem> SupplyItems { get; set; } = new List<SupplyItem>();
}
