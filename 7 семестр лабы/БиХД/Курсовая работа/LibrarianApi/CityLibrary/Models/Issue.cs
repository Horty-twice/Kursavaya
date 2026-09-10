using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class Issue
{
    public int Id { get; set; }

    public DateOnly RequestDate { get; set; }

    public DateOnly? PlannedReturnDate { get; set; }

    public int Readerid { get; set; }

    public int IssueStatusid { get; set; }

    public int Employeeid { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<IssueItem> IssueItems { get; set; } = new List<IssueItem>();

    public virtual IssueStatus IssueStatus { get; set; } = null!;

    public virtual Reader Reader { get; set; } = null!;
}
