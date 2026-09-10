using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class BookCopy
{
    public int Id { get; set; }

    public string InventoryNumber { get; set; } = null!;

    public DateOnly ArrivalDate { get; set; }

    public int Bookid { get; set; }

    public int BookCopyStatusid { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual BookCopyStatus BookCopyStatus { get; set; } = null!;
}
