using System;
using System.Collections.Generic;

namespace CityLibrary.Models;

public partial class BookCopyStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<BookCopy> BookCopies { get; set; } = new List<BookCopy>();
}
