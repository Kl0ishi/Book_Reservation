using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Book_Reservation.ViewModels
{
    public class BookViewModel
    {
        [DisplayName("ไอดีหนังสือ")]
        public int BookId { get; set; }

        [DisplayName("ชื่อหนังสือ")]
        public string? BookName { get; set; }

        [DisplayName("ไอดีประเภทหนังสือ")]
        public int? BookTypeId { get; set; }

        [DisplayName("ประเภทหนังสือ")]
        public string? BookTypeName { get; set; }

        public int? BookStockId { get; set; }
        [DisplayName("จำนวนสต็อก")]
        public int? NumStock { get;set; }
        [DisplayName("เลขISBN")]
        public string? BookIsbn { get; set; }
        [DisplayName("ต้นทุนหนังสือ")]
        public int? BookCost { get; set; }
        [DisplayName("ราคา")]
        public int? BookPrice { get; set; }
        public string? Path { get; set; }
        [NotMapped]
        [Display(Name = "Image")]
        public IFormFile ImagePath { get; set; }
    }
}
