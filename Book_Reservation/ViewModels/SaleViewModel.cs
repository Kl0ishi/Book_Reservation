using Book_Reservation.Models;

namespace Book_Reservation.ViewModels
{
    public class SaleViewModel
    {
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
        public List<SaleItemViewModel> SaleItems { get; set; }
    }
}
