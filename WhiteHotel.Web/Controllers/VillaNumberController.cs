using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteHotel.Application.Common.Interfaces;
using WhiteHotel.Web.ViewModels;

namespace WhiteHotel.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var result = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(result);
        }
        public IActionResult Create()
        {
            VillaNumberVM villaNumberVm = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(item => new SelectListItem
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
            bool roomNumberExists = _unitOfWork.VillaNumber.Any(u => u.Villa_Number == model.VillaNumber.Villa_Number);
            if (ModelState.IsValid && !roomNumberExists)
            {
                _unitOfWork.VillaNumber.Add(model.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been created successfully";
                return RedirectToAction(nameof(Index));   
            }

            if (roomNumberExists)
                TempData["error"] = "The villa number already exists.";

            model.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem()
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
                VillaList = _unitOfWork.Villa.GetAll().Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
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
                _unitOfWork.VillaNumber.Update(model.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been updated successfully";
                return RedirectToAction(nameof(Index));
            }

            model.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem()
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
                VillaList = _unitOfWork.Villa.GetAll().Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };

            if (villaNumberVm.VillaNumber == null)
                return RedirectToAction("Error", "Home");

            return View(villaNumberVm);
        }
        [HttpPost]
        public IActionResult Delete(VillaNumberVM model)
        {
            var result = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == model.VillaNumber.Villa_Number);

            if (result is not null)
            {
                _unitOfWork.VillaNumber.Remove(result);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "The villa number could not be deleted";
            return View();
        }
    }


}
