using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class Fine
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public DateOnly ImpositionDate { get; set; }

    public DateOnly? PaymentDate { get; set; }

    public int Readerid { get; set; }

    public int IssueItemid { get; set; }

    public int FineStatusid { get; set; }

    public int FineReasonid { get; set; }

    public virtual FineReason FineReason { get; set; } = null!;

    public virtual FineStatus FineStatus { get; set; } = null!;

    public virtual IssueItem IssueItem { get; set; } = null!;

    public virtual Reader Reader { get; set; } = null!;
}
