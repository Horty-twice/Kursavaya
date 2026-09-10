using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class ManagerRequest
{
    public int Id { get; set; }

    public DateOnly CreationDate { get; set; }

    public int Employeeid { get; set; }

    public int ManagerRequestStatusid { get; set; }

    public int Bookid { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual ManagerRequestStatus ManagerRequestStatus { get; set; } = null!;
}
