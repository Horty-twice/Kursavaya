using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class IssueItem
{
    public int Id { get; set; }

    public DateOnly? ActualReturnDate { get; set; }

    public int Issueid { get; set; }

    public int IssueItemStatusid { get; set; }

    public int Bookid { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();

    public virtual Issue Issue { get; set; } = null!;

    public virtual IssueItemStatus IssueItemStatus { get; set; } = null!;
}
