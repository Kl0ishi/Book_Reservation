namespace Book_Reservation.ViewModels
{
    public class SaleItemViewModel
    {
        public int SaleItemId { get; set; }
        public int BookId { get; set; }
        public int? Seq { get; set; }
        public string BookName { get; set; }
        public int? BookNum { get; set; }
        public int? BookTotal { get; set; }
        public string? InsertBy { get; set; }

        public DateTime? InsertDate { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
