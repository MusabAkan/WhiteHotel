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
                return RedirectToAction("Index");
            }
            return View();
           
        }

        public IActionResult Update(int villaId)
        {
            var result = _context.Villas.FirstOrDefault(u => u.Id == villaId);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        public IActionResult Delete()
        {
            throw new NotImplementedException();
        }
    }
}
