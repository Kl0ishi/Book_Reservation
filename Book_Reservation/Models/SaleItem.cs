using System;
using System.Collections.Generic;

namespace Book_Reservation.Models;

public partial class SaleItem
{
    public int SaleItemId { get; set; }

    public int SaleId { get; set; }

    public int BookId { get; set; }

    public int? Seq { get; set; }

    public int? BookNum { get; set; }

    public int? BookTotal { get; set; }

    public string? InsertBy { get; set; }

    public DateTime? InsertDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;
}
