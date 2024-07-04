using Book_Reservation.Dropdown;
using Book_Reservation.Interface;
using Book_Reservation.Interface.BookResult;
using Book_Reservation.Models;
using Book_Reservation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;

namespace Book_Reservation.Controllers
{
    public class BookController : Controller
    {
        private readonly ReservationDevContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly InterfaceApi _interfaceApi;

        public BookController(ReservationDevContext context, IWebHostEnvironment webHostEnvironment, InterfaceApi interfaceApi)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _interfaceApi = interfaceApi;
        }

        //private async Task<int> GetCount(string Insertby)
        //{
        //    var MultipleTable = (from a in _context.Books
        //                         join b in _context.BookTypes on a.BookTypeId equals b.BooktypeId
        //                         join c in _context.BookStocks on a.BookId equals c.BookId
        //                         where a.BookStatus == true
        //                         select new BookViewModel
        //                         {
        //                             BookName = a.BookName,
        //                             BookTypeId = a.BookTypeId,
        //                             BookTypeName = b.BooktypeName,
        //                             NumStock = c.NumStock,
        //                             BookIsbn = a.BookIsbn,
        //                             BookCost = (int)a.BookCost,
        //                             BookPrice = (int)a.BookPrice,
        //                             BookImage = a.BookImage
        //                         }).Count();
        //    return MultipleTable;
        //}

        // GET: Books
        public async Task<IActionResult> Home(int Currentpage, string searchText, int? filterType, decimal? minPrice, decimal? maxPrice)
        {
            try
            {
                // Store the search parameters in ViewBag to pass them to the view
                ViewBag.CurrentSearchText = searchText;
                ViewBag.CurrentFilterType = filterType;
                ViewBag.CurrentMinPrice = minPrice;
                ViewBag.CurrentMaxPrice = maxPrice;

                if (Currentpage == 0)
                {
                    Currentpage = 1;
                }
                if(searchText == null)
                {
                    searchText = "";
                }
                if(filterType == null) 
                { 
                    filterType = 0;
                }
                if(minPrice == null)
                {
                    minPrice = 0;
                }
                if(maxPrice == null)
                {
                    maxPrice = 999999999;
                }

                Bookparam bookparams = new Bookparam
                {
                    Currentpage = Currentpage,
                    searchText = searchText,
                    filterType = filterType,
                    minPrice = minPrice,
                    maxPrice = maxPrice
                };

                // Call the DataApi to get processed data from the Web API
                    var processedData = await _interfaceApi.Freceivebook(bookparams);

                // Render the view with the processed data
                return View(processedData.BookResponses);
            }
            catch (Exception ex)
            {
                throw;
            }
            //try
            //{
            //    // Store the search parameters in ViewBag to pass them to the view
            //    ViewBag.CurrentSearchText = searchText;
            //    ViewBag.CurrentFilterType = filterType;
            //    ViewBag.CurrentMinPrice = minPrice;
            //    ViewBag.CurrentMaxPrice = maxPrice;

            //    BookPagingViewModel BookPagingViewModels = new BookPagingViewModel();

            //    if (Currentpage == null || Currentpage == 0)
            //    {
            //        Currentpage = 1;
            //    }

            //    int maxrowsperpage;
            //    maxrowsperpage = 12;

            //    // Fetch all books from the database
            //    List<BookViewModel> allBooks = (from a in _context.Books
            //                                    join b in _context.BookTypes on a.BookTypeId equals b.BooktypeId
            //                                    join c in _context.BookStocks on a.BookId equals c.BookId
            //                                    where a.BookStatus == true
            //                                    select new BookViewModel
            //                                    {
            //                                        BookId = a.BookId,
            //                                        BookName = a.BookName,
            //                                        BookTypeId = a.BookTypeId,
            //                                        BookTypeName = b.BooktypeName,
            //                                        NumStock = c.NumStock,
            //                                        BookIsbn = a.BookIsbn,
            //                                        BookCost = (int)a.BookCost,
            //                                        BookPrice = (int)a.BookPrice,
            //                                        BookImage = a.BookImage
            //                                    }).ToList();

            //    // Apply search and filter conditions to the list of all books
            //    if (!string.IsNullOrEmpty(searchText))
            //    {
            //        // Filter by Book Name containing the search text
            //        allBooks = allBooks
            //            .Where(b => b.BookName.Contains(searchText.Trim(), StringComparison.OrdinalIgnoreCase))
            //            .ToList();
            //    }

            //    if (filterType.HasValue && filterType.Value > 0)
            //    {
            //        // Filter by Book Type
            //        allBooks = allBooks
            //            .Where(b => b.BookTypeId == filterType.Value)
            //            .ToList();
            //    }

            //    // Apply search and filter conditions to the list of all books
            //    if (minPrice.HasValue)
            //    {
            //        allBooks = allBooks
            //            .Where(b => b.BookPrice >= minPrice.Value)
            //            .ToList();
            //    }

            //    if (maxPrice.HasValue)
            //    {
            //        allBooks = allBooks
            //            .Where(b => b.BookPrice <= maxPrice.Value)
            //            .ToList();
            //    }

            //    // Calculate pagination
            //    int Scount = allBooks.Count;
            //    int totalPages = (int)Math.Ceiling((decimal)Scount / (decimal)maxrowsperpage);
            //    int startPage = Currentpage - 5;
            //    int endPage = Currentpage + 4;

            //    if (startPage <= 0)
            //    {
            //        endPage = endPage - (startPage - 1);
            //        startPage = 1;
            //    }

            //    if (endPage > totalPages)
            //    {
            //        endPage = totalPages;
            //        if (endPage > 10)
            //        {
            //            startPage = endPage - 9;
            //        }
            //    }

            //    // Fetch books for the current page
            //    BookPagingViewModels.BookViewModels = allBooks
            //        .OrderByDescending(x => x.BookId)
            //        .Skip((Currentpage - 1) * maxrowsperpage)
            //        .Take(maxrowsperpage)
            //        .ToList();

            //    BookPagingViewModels.PageCount = Scount;
            //    BookPagingViewModels.CurrentPage = Currentpage;
            //    BookPagingViewModels.PageSize = maxrowsperpage;
            //    BookPagingViewModels.TotalPages = totalPages;
            //    BookPagingViewModels.StartPage = startPage;
            //    BookPagingViewModels.EndPage = endPage;

            //    return View(BookPagingViewModels);
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }

        // GET: Books
        public async Task<IActionResult> Index(int Currentpage, string searchText, int? filterType)
        {
            try
            {
                // Store the search parameters in ViewBag to pass them to the view
                ViewBag.CurrentSearchText = searchText;
                ViewBag.CurrentFilterType = filterType;

                BookPagingViewModel BookPagingViewModels = new BookPagingViewModel();

                if (Currentpage == null || Currentpage == 0)
                {
                    Currentpage = 1;
                }

                int maxrowsperpage;
                maxrowsperpage = 12;

                // Fetch all books from the database
                List<BookViewModel> allBooks = (from a in _context.Books
                                                join b in _context.BookTypes on a.BookTypeId equals b.BooktypeId
                                                join c in _context.BookStocks on a.BookId equals c.BookId
                                                where a.BookStatus == true
                                                select new BookViewModel
                                                {
                                                    BookId = a.BookId,
                                                    BookName = a.BookName,
                                                    BookTypeId = a.BookTypeId,
                                                    BookTypeName = b.BooktypeName,
                                                    NumStock = c.NumStock,
                                                    BookIsbn = a.BookIsbn,
                                                    BookCost = (int)a.BookCost,
                                                    BookPrice = (int)a.BookPrice,
                                                    Path = a.Path
                                                }).ToList();

                // Apply search and filter conditions to the list of all books
                if (!string.IsNullOrEmpty(searchText))
                {
                    // Filter by Book Name containing the search text
                    allBooks = allBooks
                        .Where(b => b.BookName.Contains(searchText.Trim(), StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (filterType.HasValue && filterType.Value > 0)
                {
                    // Filter by Book Type
                    allBooks = allBooks
                        .Where(b => b.BookTypeId == filterType.Value)
                        .ToList();
                }

                // Calculate pagination
                int Scount = allBooks.Count;
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

                // Fetch books for the current page
                BookPagingViewModels.BookViewModels = allBooks
                    .OrderByDescending(x => x.BookId)
                    .Skip((Currentpage - 1) * maxrowsperpage)
                    .Take(maxrowsperpage)
                    .ToList();

                BookPagingViewModels.PageCount = Scount;
                BookPagingViewModels.CurrentPage = Currentpage;
                BookPagingViewModels.PageSize = maxrowsperpage;
                BookPagingViewModels.TotalPages = totalPages;
                BookPagingViewModels.StartPage = startPage;
                BookPagingViewModels.EndPage = endPage;

                return View(BookPagingViewModels);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: Books/Create
        public async Task<IActionResult> Create()
        {
            var currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.InsertDate = currentDate;
            ViewBag.UpdateDate = currentDate;

            var BookTypeDt = await (from a in _context.BookTypes
                                    where a.BooktypeStatus == true
                                    select new LBookType
                                    {
                                        BookTypeId = a.BooktypeId,
                                        BookTypeName = a.BooktypeId + " " + a.BooktypeName,
                                    }).ToListAsync().ConfigureAwait(false);

            ViewData["BookTypeDt"] = new SelectList(BookTypeDt, "BookTypeId", "BookTypeName");

            //ViewData["BookStockId"] = new SelectList(_context.BookStocks, "BookStockId", "BookStockId");
            //ViewData["BookTypeId"] = new SelectList(_context.BookTypes, "BooktypeId", "BooktypeId");


            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,BookName,BookTypeId,BookIsbn,BookCost,BookPrice,BookStatus,InsertBy,InsertDate,UpdateBy,UpdateDate,Path,ImagePath")] Book book, int numStock)
        {

            try
            {
                var BookTypeDt = await (from a in _context.BookTypes
                                        where a.BooktypeStatus == true
                                        select new LBookType
                                        {
                                            BookTypeId = a.BooktypeId,
                                            BookTypeName = a.BooktypeId + " " + a.BooktypeName,
                                        }).ToListAsync().ConfigureAwait(false);
                ViewData["BookTypeDt"] = new SelectList(BookTypeDt, "BookTypeId", "BookTypeName", book.BookTypeId);

                //Console.WriteLine($"ModelState: {ModelState.IsValid}");

                if (book.BookStatus == null)
                {
                    book.BookStatus = true;
                }

                var existingBook = await _context.Books.FirstOrDefaultAsync(b => b.BookName == book.BookName && b.BookTypeId == book.BookTypeId);

                if (existingBook != null)
                {
                    // Update existing BookStock with the provided numStock
                    var existingBookStock = await _context.BookStocks.FirstOrDefaultAsync(bs => bs.BookId == existingBook.BookId);
                    if (existingBookStock != null)
                    {
                        existingBookStock.NumStock += numStock;
                        existingBook.BookStatus = true;

                        existingBookStock.UpdateBy = book.UpdateBy;
                        existingBookStock.UpdateDate = book.UpdateDate;
                        _context.BookStocks.Update(existingBookStock);
                    }
                    else
                    {
                        // Create a new BookStock if it doesn't exist
                        var newBookStock = new BookStock
                        {
                            BookId = existingBook.BookId,
                            NumStock = numStock,
                            InsertBy = book.InsertBy,
                            InsertDate = book.InsertDate,
                            UpdateBy = book.UpdateBy,
                            UpdateDate = book.UpdateDate,
                            BookStockStatus = true
                        };
                        _context.BookStocks.Add(newBookStock);
                    }
                }

                else
                {
                    // Create a new book and a corresponding BookStock
                    _context.Books.Add(book);
                    await _context.SaveChangesAsync();

                    var newBookStock = new BookStock
                    {
                        BookId = book.BookId,
                        NumStock = numStock,
                        InsertBy = book.InsertBy,
                        InsertDate = book.InsertDate,
                        UpdateBy = book.UpdateBy,
                        UpdateDate = book.UpdateDate,
                        BookStockStatus = true
                    };
                    _context.BookStocks.Add(newBookStock);
                }

                string uniqueFileName = UploadImage(book);
                book.Path = uniqueFileName;
                _context.Books.Add(book);
                

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");


            }
            catch (Exception ex)
            {
                return View(ex);
                throw;
            }

            //return View(book);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var book = _context.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            var BookTypeDt = await (from a in _context.BookTypes
                                        //where a.BooktypeStatus == true
                                    select new LBookType
                                    {
                                        BookTypeId = a.BooktypeId,
                                        BookTypeName = a.BooktypeId + " " + a.BooktypeName + " " + (a.BooktypeStatus == true ? "(Usable)" : a.BooktypeStatus == false ? "(Not use anymore)" : ""),
                                    }).ToListAsync().ConfigureAwait(false);

            ViewData["BookTypeDt"] = new SelectList(BookTypeDt, "BookTypeId", "BookTypeName");

            //ViewData["BookStockId"] = new SelectList(_context.BookStocks, "BookStockId", "BookStockId");
            //ViewData["BookStockId"] = new SelectList(_context.BookStocks, "BookStockId", "BookStockId", book.BookStockId);
            //ViewData["BookTypeId"] = new SelectList(_context.BookTypes, "BooktypeId", "BooktypeId", book.BookTypeId);
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book obj)
        {
            var BookTypeDt = await (from a in _context.BookTypes
                                        //where a.BooktypeStatus == true
                                    select new LBookType
                                    {
                                        BookTypeId = a.BooktypeId,
                                        BookTypeName = a.BooktypeId + " " + a.BooktypeName + " " + (a.BooktypeStatus == true ? "(Usable)" : a.BooktypeStatus == false ? "(Not use anymore)" : ""),
                                    }).ToListAsync().ConfigureAwait(false);

            ViewData["BookTypeDt"] = new SelectList(BookTypeDt, "BookTypeId", "BookTypeName", obj.BookTypeId);

            if (!ModelState.IsValid)
            {
                var whatwewant = _context.Books.Find(obj.BookId);
                if (whatwewant == null)
                {
                    return NotFound();
                }

                whatwewant.BookName = obj.BookName;
                whatwewant.BookTypeId = obj.BookTypeId;
                whatwewant.BookIsbn = obj.BookIsbn;
                whatwewant.BookCost = obj.BookCost;
                whatwewant.BookPrice = obj.BookPrice;
                whatwewant.UpdateBy = obj.UpdateBy;
                whatwewant.UpdateDate = obj.UpdateDate;

                string uniqueFileName = string.Empty;
                if (obj.ImagePath != null)
                {
                    if (whatwewant.Path != null)
                    {
                        //แทนที่รูปอันเก่า
                        string filepath = Path.Combine(_webHostEnvironment.WebRootPath, "Images/Books/", whatwewant.Path);
                        if (System.IO.File.Exists(filepath))
                        {
                            System.IO.File.Delete(filepath);
                        }
                    }
                    uniqueFileName = UploadImage(obj);
                    whatwewant.Path = uniqueFileName;

                    //_context.Books.Update(obj);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }
            return View(obj);
        }
        //private bool BookExists(int id)
        //{
        //    return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
        //}

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.BookType)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _context.Books.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //ค้นข้อมูล
            var obj = _context.Books.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            //_context.Books.Remove(obj);
            obj.BookStatus = false;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private string UploadImage(Book obj)
        {
            string uniqueFileName = string.Empty;
            if (obj.ImagePath != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images/Books/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + obj.ImagePath.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    obj.ImagePath.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
