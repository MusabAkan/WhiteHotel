using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteHotel.Application.Common.Interfaces;
using WhiteHotel.Application.Common.Utility;
using WhiteHotel.Web.ViewModels;

namespace WhiteHotel.Web.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class AmenityController : Controller
    {
        readonly IUnitOfWork _unitOfWork;

        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var result = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(result);
        }
        public IActionResult Create()
        {
            AmenityVM AmenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                })
            };
            return View(AmenityVM);
        }
        [HttpPost]
        public IActionResult Create(AmenityVM model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Add(model.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The amenity has been created successfully";
                return RedirectToAction(nameof(Index));
            }

            model.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem()
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(model);
        }
        public IActionResult Update(int amenityId)
        {
            AmenityVM AmenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityId)
            };

            if (AmenityVM.Amenity == null)
                return RedirectToAction("Error", "Home");

            return View(AmenityVM);
        }
        [HttpPost]
        public IActionResult Update(AmenityVM model)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(model.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The amenity has been updated successfully";
                return RedirectToAction(nameof(Index));
            }

            model.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem()
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(model);
        }
        public IActionResult Delete(int amenityId)
        {
            AmenityVM AmenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityId)
            };

            if (AmenityVM.Amenity == null)
                return RedirectToAction("Error", "Home");

            return View(AmenityVM);
        }
        [HttpPost]
        public IActionResult Delete(AmenityVM model)
        {
            var result = _unitOfWork.Amenity.Get(u => u.Id == model.Amenity.Id);

            if (result is not null)
            {
                _unitOfWork.Amenity.Remove(result);
                _unitOfWork.Save();
                TempData["success"] = "The amenity has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "The amenity could not be deleted";
            return View();
        }
    }


}
