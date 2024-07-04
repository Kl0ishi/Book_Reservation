using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Reservation.Models;

public partial class Book
{
    [Key]
    public int BookId { get; set; }

    [Required(ErrorMessage ="กรุณากรอกชื่อหนังสือ")]
    [DisplayName("ชื่อหนังสือ")]
    public string BookName { get; set; }

    [Required(ErrorMessage = "กรุณากรอกชื่อหนังสือ")]
    public int BookTypeId { get; set; }

    public string? BookIsbn { get; set; }

    public int? BookCost { get; set; }

    public int? BookPrice { get; set; }

    public bool? BookStatus { get; set; }

    public string? InsertBy { get; set; }

    public DateTime? InsertDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    [NotMapped] // This attribute tells EF Core to ignore this property when mapping to the database
    [Display(Name = "Choose Image")]
    public IFormFile ImagePath { get; set; }
    public string? Path { get; set; }

    public virtual BookType BookType { get; set; } = null!;

    public virtual ICollection<BookStock> BookStocks { get; set; } = new List<BookStock>();

    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
