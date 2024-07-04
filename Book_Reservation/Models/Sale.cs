using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Book_Reservation.Models;

public partial class Sale
{
    [Key]
    public int SaleId { get; set; }

    public string? SaleCode { get; set; }

    public int PersonId { get; set; }

    public int? SaleNum { get; set; }

    public int? SalePrice { get; set; }

    public int? SaleDiscount { get; set; }

    public int? SaleTotal { get; set; }

    public int StatusId { get; set; }

    public string? InsertBy { get; set; }

    public DateTime? InsertDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

    public virtual Status Status { get; set; } = null!;
}
