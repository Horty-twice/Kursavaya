using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class PurchaseRequest
{
    public int Id { get; set; }

    public DateOnly CreationDate { get; set; }

    public int Supplierid { get; set; }

    public int PurchaseRequestStatusid { get; set; }

    public virtual ICollection<PurchaseRequestItem> PurchaseRequestItems { get; set; } = new List<PurchaseRequestItem>();

    public virtual PurchaseRequestStatus PurchaseRequestStatus { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
