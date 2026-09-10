using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class IssueStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();
}
