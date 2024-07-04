using Book_Reservation.Models;
using Book_Reservation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Book_Reservation.Controllers
{
    public class SaleController : Controller
    {
        private readonly ReservationDevContext _context;

        private List<Book> cartItems;

        public SaleController(ReservationDevContext context)
        {
            _context = context;
            cartItems = new List<Book>();
        }
        private async Task<int> GetCount(string Insertby)
        {
            var MultipleTable = (from a in _context.Sales
                                 join b in _context.Statuses on a.StatusId equals b.StatusId
                                 select new VeiwModelSale
                                 {
                                     InsertDate = (DateTime)a.InsertDate,
                                     SaleCode = a.SaleCode,
                                     StatusId = a.StatusId,
                                     StatusName = b.StatusName,
                                     SaleTotal = (int)a.SaleTotal,
                                     SaleNum = (int)a.SaleNum
                                 }
                ).Count();
            return MultipleTable;
        }
        public async Task<IActionResult> Index(int Currentpage)
        {
            try
            {
                ViewModelSalepaging viewModelSalepagings = new ViewModelSalepaging();

                if (Currentpage == null || Currentpage == 0)
                {
                    Currentpage = 1;
                }

                int maxrowsperpage;
                maxrowsperpage = 5;


                viewModelSalepagings.veiwModelSales = (from a in _context.Sales
                                                       join b in _context.Statuses on a.StatusId equals b.StatusId
                                                       select new VeiwModelSale
                                                       {
                                                           SaleId = a.SaleId,
                                                           InsertDate = (DateTime)a.InsertDate,
                                                           SaleCode = a.SaleCode,
                                                           StatusId = a.StatusId,
                                                           StatusName = b.StatusName,
                                                           SaleTotal = (int)a.SaleTotal,
                                                           SaleNum = (int)a.SaleNum
                                                       }).OrderByDescending(x => x.SaleId)
                                                       .Skip((Currentpage - 1) * maxrowsperpage)
                                                                .Take(maxrowsperpage)
                                                                .ToList();

                int Scount = await GetCount("008433");


                int totalPages = (int)Math.Ceiling((decimal)Scount / (decimal)maxrowsperpage);
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

                viewModelSalepagings.PageCount = Scount;
                viewModelSalepagings.CurrentPage = Currentpage;
                viewModelSalepagings.PageSize = maxrowsperpage;
                viewModelSalepagings.TotalPages = totalPages;
                viewModelSalepagings.StartPage = startPage;
                viewModelSalepagings.EndPage = endPage;

                return View(viewModelSalepagings);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<IActionResult> Create()
        {
            var currentDate = DateTime.Now;
            string newOrder = GenerateNumber();

            //var reservationDevContext = _context.Books.Include(b => b.BookType).Where(b => b.BookStatus == true);
            //return View(await reservationDevContext.ToListAsync());

            ViewBag.SaleCode = newOrder;
            ViewBag.currentDate = currentDate.ToString("dd-MM-yyyy HH:mm:ss");
            var multipleTable = await (from a in _context.Books
                                       join b in _context.BookTypes on a.BookTypeId equals b.BooktypeId
                                       where a.BookStatus == true
                                       select new BookViewModel
                                       {
                                           BookId = a.BookId,
                                           BookName = a.BookName,
                                           BookTypeName = b.BooktypeName,
                                           BookIsbn = a.BookIsbn,
                                           BookPrice = a.BookPrice
                                       }).ToListAsync();

            return View(multipleTable);
        }

        [HttpPost]
        public IActionResult Create(string SaleCode,string PurchaseDate, int SaleNum, int SaleTotal, List<SaleItem> items)
        {
            DateTime parsedPurchaseDate = DateTime.ParseExact(PurchaseDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            Sale receiptModel = new Sale
            {
                InsertDate = parsedPurchaseDate,
                PersonId = 2,
                StatusId = 2,
                SaleNum = SaleNum,
                SaleTotal = SaleTotal,
                SaleCode = SaleCode
            };
            _context.Sales.Add(receiptModel);
            _context.SaveChanges();

            foreach(var item in items)
            {
                SaleItem saleItem = new SaleItem
                {
                    BookId = item.BookId,
                    SaleId = receiptModel.SaleId,
                    BookNum = item.BookNum,
                    BookTotal = item.BookTotal,
                    InsertDate = parsedPurchaseDate
                };
                _context.SaleItems.Add(saleItem);
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var saleDetails = await (from a in _context.Sales
                                       join b in _context.SaleItems on a.SaleId equals b.SaleId
                                       join c in _context.Books on b.BookId equals c.BookId
                                       where a.SaleId == id
                                       select new SaleViewModel
                                       {
                                           SaleId = a.SaleId,
                                           SaleCode = a.SaleCode,
                                           InsertDate = a.InsertDate,
                                           SaleTotal = a.SaleTotal,
                                           SaleNum = a.SaleNum,
                                           SaleItems = (from b in _context.SaleItems
                                                        where b.SaleId == a.SaleId
                                                        join c in _context.Books on b.BookId equals c.BookId
                                                        select new SaleItemViewModel
                                                        {
                                                            SaleItemId = b.SaleItemId,
                                                            BookId = b.BookId,
                                                            BookName = c.BookName,
                                                            BookNum = b.BookNum,
                                                            BookTotal = b.BookTotal,
                                                            InsertDate = b.InsertDate
                                                        }).ToList()
                                       }).FirstOrDefaultAsync();

            if (saleDetails == null)
            {
                return NotFound();
            }

            return View(saleDetails);
        }

        public IActionResult Cancel(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var sale = _context.Sales
               .Include(s => s.Person)
               .Include(s => s.Status)
               .FirstOrDefault(s => s.SaleId == id);

            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            //ค้นข้อมูล
            var obj = _context.Sales.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            
            obj.StatusId = 3;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private async Task<int> GetCount2(string Insertby)
        {
            var MultipleTable = (from a in _context.Sales
                                 join b in _context.Statuses on a.StatusId equals b.StatusId
                                 where a.StatusId == 2
                                 select new VeiwModelSale
                                 {
                                     InsertDate = (DateTime)a.InsertDate,
                                     SaleCode = a.SaleCode,
                                     StatusId = a.StatusId,
                                     StatusName = b.StatusName,
                                     SaleTotal = (int)a.SaleTotal,
                                     SaleNum = (int)a.SaleNum
                                 }
                ).Count();
            return MultipleTable;
        }

        public async Task<IActionResult> SaleList(int Currentpage)
        {
            try
            {
                ViewModelSalepaging viewModelSalepagings = new ViewModelSalepaging();

                if (Currentpage == null || Currentpage == 0)
                {
                    Currentpage = 1;
                }

                int maxrowsperpage;
                maxrowsperpage = 5;


                viewModelSalepagings.veiwModelSales = (from a in _context.Sales
                                                       join b in _context.Statuses on a.StatusId equals b.StatusId
                                                       where a.StatusId == 2
                                                       select new VeiwModelSale
                                                       {
                                                           SaleId = a.SaleId,
                                                           InsertDate = (DateTime)a.InsertDate,
                                                           SaleCode = a.SaleCode,
                                                           StatusId = a.StatusId,
                                                           StatusName = b.StatusName,
                                                           SaleTotal = (int)a.SaleTotal,
                                                           SaleNum = (int)a.SaleNum
                                                       }).OrderByDescending(x => x.SaleId)
                                                       .Skip((Currentpage - 1) * maxrowsperpage)
                                                                .Take(maxrowsperpage)
                                                                .ToList();

                int Scount = await GetCount2("");


                int totalPages = (int)Math.Ceiling((decimal)Scount / (decimal)maxrowsperpage);
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

                viewModelSalepagings.PageCount = Scount;
                viewModelSalepagings.CurrentPage = Currentpage;
                viewModelSalepagings.PageSize = maxrowsperpage;
                viewModelSalepagings.TotalPages = totalPages;
                viewModelSalepagings.StartPage = startPage;
                viewModelSalepagings.EndPage = endPage;

                return View(viewModelSalepagings);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<IActionResult> DeliveryConfirm(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var saleDetails = await (from a in _context.Sales
                                     join b in _context.SaleItems on a.SaleId equals b.SaleId
                                     join c in _context.Books on b.BookId equals c.BookId
                                     where a.SaleId == id
                                     select new SaleViewModel
                                     {
                                         SaleId = a.SaleId,
                                         SaleCode = a.SaleCode,
                                         InsertDate = a.InsertDate,
                                         SaleTotal = a.SaleTotal,
                                         SaleNum = a.SaleNum,
                                         SaleItems = (from b in _context.SaleItems
                                                      where b.SaleId == a.SaleId
                                                      join c in _context.Books on b.BookId equals c.BookId
                                                      select new SaleItemViewModel
                                                      {
                                                          SaleItemId = b.SaleItemId,
                                                          BookId = b.BookId,
                                                          BookName = c.BookName,
                                                          BookNum = b.BookNum,
                                                          BookTotal = b.BookTotal,
                                                          InsertDate = b.InsertDate
                                                      }).ToList()
                                     }).FirstOrDefaultAsync();

            if (saleDetails == null)
            {
                return NotFound();
            }

            return View(saleDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeliveryConfirm(int id)
        {
            //ค้นข้อมูล
            var obj = _context.Sales.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            obj.StatusId = 5;
            _context.SaveChanges();
            return RedirectToAction("SaleList");
        }

        private async Task<int> GetCount3(string Insertby)
        {
            var MultipleTable = (from a in _context.Sales
                                 join b in _context.Statuses on a.StatusId equals b.StatusId
                                 join c in _context.People on a.PersonId equals c.PersonId
                                 where a.StatusId == 5
                                 select new VeiwModelSale
                                 {
                                     InsertDate = (DateTime)a.InsertDate,
                                     SaleCode = a.SaleCode,
                                     StatusId = a.StatusId,
                                     StatusName = b.StatusName,
                                     PersonId = a.PersonId,
                                     PersonName = c.PersonName,
                                     SaleTotal = (int)a.SaleTotal,
                                     SaleNum = (int)a.SaleNum
                                 }
                ).Count();
            return MultipleTable;
        }

        public IActionResult UserCancel(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var sale = _context.Sales
               .Include(s => s.Person)
               .Include(s => s.Status)
               .FirstOrDefault(s => s.SaleId == id);

            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserCancel(int id)
        {
            //ค้นข้อมูล
            var obj = _context.Sales.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            obj.StatusId = 3;
            _context.SaveChanges();
            return RedirectToAction("SaleList");
        }

        public async Task<IActionResult> DeliveryList(int Currentpage)
        {
            try
            {
                ViewModelSalepaging viewModelSalepagings = new ViewModelSalepaging();

                if (Currentpage == null || Currentpage == 0)
                {
                    Currentpage = 1;
                }

                int maxrowsperpage;
                maxrowsperpage = 5;


                viewModelSalepagings.veiwModelSales = (from a in _context.Sales
                                                       join b in _context.Statuses on a.StatusId equals b.StatusId
                                                       join c in _context.People on a.PersonId equals c.PersonId
                                                       where a.StatusId == 5
                                                       select new VeiwModelSale
                                                       {
                                                           SaleId = a.SaleId,
                                                           InsertDate = (DateTime)a.InsertDate,
                                                           SaleCode = a.SaleCode,
                                                           StatusId = a.StatusId,
                                                           StatusName = b.StatusName,
                                                           PersonId = a.PersonId,
                                                           PersonName = c.PersonName,
                                                           SaleTotal = (int)a.SaleTotal,
                                                           SaleNum = (int)a.SaleNum
                                                       }).OrderByDescending(x => x.SaleId)
                                                       .Skip((Currentpage - 1) * maxrowsperpage)
                                                                .Take(maxrowsperpage)
                                                                .ToList();

                int Scount = await GetCount3("");


                int totalPages = (int)Math.Ceiling((decimal)Scount / (decimal)maxrowsperpage);
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

                viewModelSalepagings.PageCount = Scount;
                viewModelSalepagings.CurrentPage = Currentpage;
                viewModelSalepagings.PageSize = maxrowsperpage;
                viewModelSalepagings.TotalPages = totalPages;
                viewModelSalepagings.StartPage = startPage;
                viewModelSalepagings.EndPage = endPage;

                return View(viewModelSalepagings);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private string GenerateNumber()
        {
            var strPrefix = "NO";

            var currentDate = DateTime.Now;
            var currentYear = currentDate.Year;
            var currentMonth = currentDate.Month;

            var generateNumber = _context.GenerateNumbers
                .FirstOrDefault(g => g.Year == currentYear && g.Month == currentMonth);

            if(generateNumber == null)
            {
                generateNumber = new GenerateNumber()
                {
                    Year = currentYear,
                    Month = currentMonth,
                    Sequence = 1
                };
                _context.GenerateNumbers.Add(generateNumber);
            }
            else
            {
                generateNumber.Sequence++;
                _context.Update(generateNumber);
            }

            _context.SaveChanges();

            var saleCode = $"{strPrefix}-{currentYear}-{currentMonth.ToString("D2")}-{generateNumber.Sequence}";
            return saleCode;
        }
       
    }
}
