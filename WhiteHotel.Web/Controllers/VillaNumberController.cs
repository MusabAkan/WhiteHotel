using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; 
using WhiteHotel.Infrastructure.Data;
using WhiteHotel.Web.ViewModels;

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
            var result = _context.VillaNumbers.Include(u => u.Villa).ToList();
            return View(result);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumberVm = new()
            {
                VillaList = _context.Villas.ToList().Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                })
            };
            return View(villaNumberVm);
        }
        [HttpPost]
        public IActionResult Create(VillaNumberVM model)
        {
            //ModelState.Remove("Villa");
            bool roomNumberExists = _context.VillaNumbers.Any(u => u.Villa_Number == model.VillaNumber.Villa_Number);
            if (ModelState.IsValid && !roomNumberExists)
            {
                _context.VillaNumbers.Add(model.VillaNumber);
                _context.SaveChanges();
                TempData["success"] = "The villa number has been created successfully";
                return RedirectToAction("Index");
            }

            if (roomNumberExists)
                TempData["error"] = "The villa number already exists.";

            model.VillaList = _context.Villas.ToList().Select(u => new SelectListItem()
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(model);
        }
        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVm = new()
            {
                VillaList = _context.Villas.ToList().Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                }),
                VillaNumber = _context.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId)
            };

            if (villaNumberVm.VillaNumber == null)
                return RedirectToAction("Error", "Home");

            return View(villaNumberVm);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM model)
        {

            if (ModelState.IsValid)
            {
                _context.VillaNumbers.Update(model.VillaNumber);
                _context.SaveChanges();
                TempData["success"] = "The villa number has been updated successfully";
                return RedirectToAction("Index");
            }

            model.VillaList = _context.Villas.ToList().Select(u => new SelectListItem()
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(model);
        }

        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM villaNumberVm = new()
            {
                VillaList = _context.Villas.ToList().Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                }),
                VillaNumber = _context.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId)
            };

            if (villaNumberVm.VillaNumber == null)
                return RedirectToAction("Error", "Home");

            return View(villaNumberVm);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM model)
        {
            var result = _context.VillaNumbers.FirstOrDefault(u => u.Villa_Number == model.VillaNumber.Villa_Number);

            if (result is not null)
            {
                _context.VillaNumbers.Remove(result);
                _context.SaveChanges();
                TempData["success"] = "The villa number has been deleted successfully";
                return RedirectToAction("Index");
            }

            TempData["error"] = "The villa number could not be deleted";
            return View();
        }
    }


}
