namespace Book_Reservation.ViewModels
{
    public class VeiwModelSale
    {
        public int SaleId { get; set; }

        public string SaleCode { get; set; }

        public DateTime? InsertDate { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public int? SaleNum { get; set; }

        public int? SaleTotal { get; set; }

        public int PersonId { get; set; }
        public string? PersonName { get; set; }
    }
}
