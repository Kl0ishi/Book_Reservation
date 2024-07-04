namespace Book_Reservation.Interface.BookResult
{
    public class Bookparam
    {
        public int Currentpage { get; set; }
        public string searchText { get; set; }
        public int? filterType { get; set; }
        public decimal? minPrice { get; set; }
        public decimal? maxPrice { get; set; }
    }
}
