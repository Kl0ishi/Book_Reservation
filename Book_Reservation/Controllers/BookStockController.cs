using Book_Reservation.Models;
using Book_Reservation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Book_Reservation.Controllers
{
    public class BookStockController : Controller
    {
        private readonly ReservationDevContext _context;
        public BookStockController(ReservationDevContext context)
        {
            _context = context;
        }

        private async Task<int> GetCount()
        {
            var MultipleTable = (from a in _context.BookStocks
                                 join b in _context.Books on a.BookId equals b.BookId
                                 select new BookStockViewModel
                                 {
                                     BookStockId = a.BookStockId,
                                     BookId = a.BookId,
                                     BookName = b.BookName,
                                     NumStock = a.NumStock,
                                     InsertBy = a.InsertBy,
                                     InsertDate = a.InsertDate,
                                     UpdateBy = a.UpdateBy,
                                     UpdateDate = a.UpdateDate
                                 }).Count();
            return MultipleTable;
        }

        public async Task<IActionResult> Index(int Currentpage)
        {
            try
            {
                BookStockPagingViewModel BookStockPagingViewModels = new BookStockPagingViewModel();

                if(Currentpage == null || Currentpage == 0)
                {
                    Currentpage = 1;
                }

                int maxrowsperpage;
                maxrowsperpage = 5;

                BookStockPagingViewModels.BookStockViewModels = (from a in _context.BookStocks
                                                                 join b in _context.Books on a.BookId equals b.BookId
                                                                 select new BookStockViewModel
                                                                 {
                                                                     BookStockId = a.BookStockId,
                                                                     BookId = a.BookId,
                                                                     BookName = b.BookName,
                                                                     NumStock = a.NumStock,
                                                                     InsertBy = a.InsertBy,
                                                                     InsertDate = a.InsertDate,
                                                                     UpdateBy = a.UpdateBy,
                                                                     UpdateDate = a.UpdateDate
                                                                 }).OrderByDescending(x => x.BookStockId)
                                                       .Skip((Currentpage - 1) * maxrowsperpage)
                                                       .Take(maxrowsperpage)
                                                       .ToList();

                int Xcount = await GetCount();

                int totalPages = (int)Math.Ceiling((decimal)Xcount / (decimal)maxrowsperpage);
                int startPage = Currentpage - 5;
                int endPage = Currentpage + 4;

                if (startPage <= 0)
                {
                    endPage = endPage - (startPage - 1);
                    startPage = 1;
                }

                if (endPage > totalPages)
                {
                    endPage = totalPages;
                    if (endPage > 10)
                    {
                        startPage = endPage - 9;
                    }
                }

                BookStockPagingViewModels.PageCount = Xcount;
                BookStockPagingViewModels.CurrentPage = Currentpage;
                BookStockPagingViewModels.PageSize = maxrowsperpage;
                BookStockPagingViewModels.TotalPages = totalPages;
                BookStockPagingViewModels.StartPage = startPage;
                BookStockPagingViewModels.EndPage = endPage;

                return View(BookStockPagingViewModels);
            }
            catch(Exception ex)
            {
                throw;
            }

            //var bookstock = _context.BookStocks.Where(c => c.BookStockStatus == true);
            //return View(await bookstock.ToListAsync());
        }

        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(BookStock obj)
        //{
        //    _context.BookStocks.Add(obj);
        //    _context.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public IActionResult Edit(int? id)
        {
            var currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.UpdateDate = currentDate;

            if(id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _context.BookStocks.Find(id);
            if(obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, BookStock obj)
        {
            var existingObj = _context.BookStocks.Find(id);
            if(existingObj == null)
            {
                return NotFound();
            }

            existingObj.NumStock = obj.NumStock;
            existingObj.UpdateBy = obj.UpdateBy;
            existingObj.UpdateDate = obj.UpdateDate;

            _context.BookStocks.Update(existingObj);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.BookStocks == null)
        //    {
        //        return NotFound();
        //    }

        //    var bookstock = await _context.BookStocks
        //        .FirstOrDefaultAsync(m => m.BookStockId == id);
        //    if(bookstock == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(bookstock);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    if(_context.BookStocks == null)
        //    {
        //        return Problem("Entity set 'ReservationDevContext.BookStocks'  is null.");
        //    }
        //    var bookstock = await _context.BookStocks.FindAsync(id);
        //    if(bookstock != null)
        //    {
        //        bookstock.BookStockStatus = false;
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        public async Task<IActionResult> Details(int id)
        {
            if(id == null || id ==0)
            {
                return NotFound();
            }
            var obj = await _context.BookStocks
                .FirstOrDefaultAsync(m => m.BookStockId==id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
    }
}
