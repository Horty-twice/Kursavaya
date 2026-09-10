using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class ManagerRequestStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ManagerRequest> ManagerRequests { get; set; } = new List<ManagerRequest>();
}
