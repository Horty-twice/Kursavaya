using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int TotalQuantity { get; set; }

    public int AvailableQuantity { get; set; }

    public int Genreid { get; set; }

    public virtual ICollection<BookCopy> BookCopies { get; set; } = new List<BookCopy>();

    public virtual Genre Genre { get; set; } = null!;

    public virtual ICollection<IssueItem> IssueItems { get; set; } = new List<IssueItem>();

    public virtual ICollection<ManagerRequest> ManagerRequests { get; set; } = new List<ManagerRequest>();

    public virtual ICollection<PurchaseRequestItem> PurchaseRequestItems { get; set; } = new List<PurchaseRequestItem>();

    public virtual ICollection<SupplyItem> SupplyItems { get; set; } = new List<SupplyItem>();

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();
}
