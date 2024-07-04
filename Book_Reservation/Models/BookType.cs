using System;
using System.Collections.Generic;

namespace Book_Reservation.Models;

public partial class BookType
{
    public int BooktypeId { get; set; }

    public string? BooktypeName { get; set; }

    public bool? BooktypeStatus { get; set; }

    public string? InsertBy { get; set; }

    public DateTime? InsertDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
