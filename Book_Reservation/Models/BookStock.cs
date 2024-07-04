using System;
using System.Collections.Generic;

namespace Book_Reservation.Models;

public partial class BookStock
{
    public int BookStockId { get; set; }

    public int BookId { get; set; }

    public int? NumStock { get; set; }

    public string? InsertBy { get; set; }

    public DateTime? InsertDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? BookStockStatus { get; set; }

    public virtual Book? Book { get; set; }

}
