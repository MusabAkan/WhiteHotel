using Microsoft.AspNetCore.Mvc;
using WhiteHotel.Domain.Entities;
using WhiteHotel.Infrastructure.Data;

namespace WhiteHotel.Web.Controllers
{

    public class VillaController : Controller
    {
        readonly ApplicationDbContext _context;

        public VillaController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var result = _context.Villas.ToList();
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa model)
        {
            if (model.Name == model.Description)
            {
                ModelState.AddModelError("name", "The description cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                _context.Villas.Add(model);
                _context.SaveChanges();
                TempData["success"] = "The villa has been created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Update(int villaId)
        {
            var result = _context.Villas.FirstOrDefault(u => u.Id == villaId);
            if (result == null)
                return RedirectToAction("Error","Home");
            return View(result);
        }
        [HttpPost]
        public IActionResult Update(Villa model)
        {
            if (ModelState.IsValid && model.Id > default(int)) 
            {
                _context.Villas.Update(model);
                _context.SaveChanges();
                TempData["success"] = "The villa has been updated successfully";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Error", "Home");
        }
        public IActionResult Delete(int villaId)
        {
            var result = _context.Villas.FirstOrDefault(u => u.Id == villaId);
            if (result == null)
                return RedirectToAction("Error", "Home");
            return View(result);
        }
        [HttpPost]
        public IActionResult Delete(Villa model)
        {
            if (ModelState.IsValid)
            {
                _context.Villas.Remove(model);
                _context.SaveChanges();
                TempData["success"] = "The villa has been deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa could not be deleted";
            return RedirectToAction("Error", "Home");
        }

     
    }
}
