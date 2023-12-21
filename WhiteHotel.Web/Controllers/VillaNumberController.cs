using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteHotel.Domain.Entities;
using WhiteHotel.Infrastructure.Data;

namespace WhiteHotel.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        readonly ApplicationDbContext _context;

        public VillaNumberController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var result = _context.VillaNumbers.ToList();
            return View(result);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> list = _context.Villas.ToList().Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString()
            });
            ViewBag.VillaList = list;
            return View();
        }
        [HttpPost]
        public IActionResult Create(VillaNumber model)
        {
            //ModelState.Remove("Villa");
            if (ModelState.IsValid)
            {
                _context.VillaNumbers.Add(model);
                _context.SaveChanges();
                TempData["success"] = "The villa number has been created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
