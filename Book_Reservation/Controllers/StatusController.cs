using Book_Reservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Book_Reservation.Controllers
{
    public class StatusController : Controller
    {
        private readonly ReservationDevContext _db;
        public StatusController(ReservationDevContext db) 
        { 
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable <Status> Status = _db.Statuses;
            return View(Status);
        }

        //GET METHOD
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Status obj)
        {
            if (ModelState.IsValid)
            {
                _db.Statuses.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Statuses.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Status obj)
        {
            if (ModelState.IsValid)
            {
                _db.Statuses.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Details(int? id)
        {
            if (id == null || _db.Statuses == null)
            {
                return NotFound();
            }

            var obj = _db.Statuses
                .FirstOrDefault(m => m.StatusId == id);
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
            var obj = _db.Statuses.Find(id);
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
            var obj = _db.Statuses.Find(id);
            if (obj != null)
            {
                _db.Statuses.Remove(obj);
            }

            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
