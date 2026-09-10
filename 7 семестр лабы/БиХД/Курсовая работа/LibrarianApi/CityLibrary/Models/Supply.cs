using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class Supply
{
    public int Id { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public decimal TotalCost { get; set; }

    public int Supplierid { get; set; }

    public int SupplyStatusid { get; set; }

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual ICollection<SupplyItem> SupplyItems { get; set; } = new List<SupplyItem>();

    public virtual SupplyStatus SupplyStatus { get; set; } = null!;
}
