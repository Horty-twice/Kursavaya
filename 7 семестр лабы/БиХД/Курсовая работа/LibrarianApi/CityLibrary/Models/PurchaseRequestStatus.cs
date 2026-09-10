using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class PurchaseRequestStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<PurchaseRequest> PurchaseRequests { get; set; } = new List<PurchaseRequest>();
}
