using System;
using System.Collections.Generic;

namespace Book_Reservation.Models;

public partial class Person
{
    public int PersonId { get; set; }
    public string? PersonCode { get; set; }
    public string? Password { get; set; }
    public string? Salt { get; set; }

    public string? PersonName { get; set; }

    public string? PersonTel { get; set; }

    public string? PersonAddress { get; set; }

    public string? InsertBy { get; set; }

    public DateTime? InsertDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
