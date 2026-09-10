using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class IssueItemStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<IssueItem> IssueItems { get; set; } = new List<IssueItem>();
}
