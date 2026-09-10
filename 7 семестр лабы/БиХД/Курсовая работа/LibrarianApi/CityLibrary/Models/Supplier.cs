using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class Supplier
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string BuildingNumber { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int Rating { get; set; }

    public virtual ICollection<PurchaseRequest> PurchaseRequests { get; set; } = new List<PurchaseRequest>();

    public virtual ICollection<Supply> Supplies { get; set; } = new List<Supply>();
}
