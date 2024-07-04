using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Book_Reservation.Interface.BookResult
{
    public class Resultbookitem
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public BookResponse BookResponses { get; set; }
    }

    public class BookResponse
    {
        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public int PageSize { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalItems { get; set; }

        public List<BookInsidePaging> BookInsidePagings { get; set; }
    }

    public class BookInsidePaging
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
        public int? NumStock { get; set; }
        [DisplayName("เลขISBN")]
        public string? BookIsbn { get; set; }
        [DisplayName("ต้นทุนหนังสือ")]
        public int? BookCost { get; set; }
        [DisplayName("ราคา")]
        public int? BookPrice { get; set; }
        public string? Path { get; set; }
        [NotMapped]
        public IFormFile ImagePath { get; set; }
    }
}
