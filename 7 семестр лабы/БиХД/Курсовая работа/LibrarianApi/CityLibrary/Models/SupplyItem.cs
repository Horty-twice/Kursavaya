using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class SupplyItem
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public decimal PricePerUnit { get; set; }

    public int Supplyid { get; set; }

    public int SupplyItemStatusid { get; set; }

    public int Bookid { get; set; }

    public int PurchaseRequestItemid { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual PurchaseRequestItem PurchaseRequestItem { get; set; } = null!;

    public virtual Supply Supply { get; set; } = null!;

    public virtual SupplyItemStatus SupplyItemStatus { get; set; } = null!;
}
