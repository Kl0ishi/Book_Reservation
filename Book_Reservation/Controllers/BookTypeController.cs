using Book_Reservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Book_Reservation.Controllers
{
    public class BookTypeController : Controller
    {
        private readonly ReservationDevContext _db;
        public BookTypeController(ReservationDevContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable <BookType> BookType = _db.BookTypes.Where(bt => bt.BooktypeStatus == true);
            return View(BookType);
        }

        //GET METHOD
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookType obj)
        {
            if (ModelState.IsValid)
            {
                _db.BookTypes.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.BookTypes.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookType obj)
        {
            if (ModelState.IsValid)
            {
                _db.BookTypes.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.BookTypes == null)
            {
                return NotFound();
            }

            var obj = await _db.BookTypes
                .FirstOrDefaultAsync(m => m.BooktypeId == id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //ค้นข้อมูล
            var obj = _db.BookTypes.Find(id);
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
            var obj = _db.BookTypes.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            //_db.BookTypes.Remove(obj);
            obj.BooktypeStatus = false;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
