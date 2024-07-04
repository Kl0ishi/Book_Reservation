using Book_Reservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFDatabaseFirst04.Controllers
{
    public class SaleItemController : Controller
    {
        private readonly ReservationDevContext _context;
        public SaleItemController(ReservationDevContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var reservationDevContext = _context.SaleItems.Include(b => b.Book).Include(b => b.Sale);
            return View(await reservationDevContext.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SaleItem obj)
        {
            _context.SaleItems.Add(obj);
            _context.Database.OpenConnection();
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT SaleItem ON");
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _context.SaleItems.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SaleItem obj)
        {
            if (!ModelState.IsValid)
            {
                _context.SaleItems.Update(obj);
                _context.Database.OpenConnection();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Book ON");
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SaleItems == null)
            {
                return NotFound();
            }

            var book = await _context.SaleItems
                .Include(b => b.Book)
                .Include(b => b.Sale)
                .FirstOrDefaultAsync(m => m.SaleItemId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.SaleItems == null)
            {
                return Problem("Entity set 'ReservationDevContext.Books'  is null.");
            }
            var Item = await _context.SaleItems.FindAsync(id);
            if (Item != null)
            {
                _context.SaleItems.Remove(Item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
