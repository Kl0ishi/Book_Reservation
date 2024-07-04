namespace Book_Reservation.ViewModels
{
    public class BookStockViewModel
    {
        public int BookStockId { get; set; }

        public int BookId { get; set; }

        public string BookName { get; set; }

        public int? NumStock { get; set; }

        public string? InsertBy { get; set; }

        public DateTime? InsertDate { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool? BookStockStatus { get; set; }

    }
}
