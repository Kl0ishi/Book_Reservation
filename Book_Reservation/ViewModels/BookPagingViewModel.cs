namespace Book_Reservation.ViewModels
{
    public class BookPagingViewModel
    {
        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public int PageSize { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalItems { get; set; }

        public List<BookViewModel> BookViewModels { get; set; }
    }
}
