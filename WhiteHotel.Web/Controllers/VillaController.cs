using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhiteHotel.Application.Common.Interfaces;
using WhiteHotel.Application.Common.Utility;
using WhiteHotel.Domain.Entities;

namespace WhiteHotel.Web.Controllers
{
    [Authorize]
    public class VillaController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IWebHostEnvironment _webHostEnvironment;

        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var result = _unitOfWork.Villa.GetAll();
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
                if (model.Image != null)
                {
                   
                    string fileName = Convert.ToString(Guid.NewGuid()) + Path.GetExtension(model.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);

                    model.Image.CopyTo(fileStream);
                    model.ImageUrl = @"\images\VillaImage\" + fileName;
                }
                else
                    model.ImageUrl = "https://placehold.co/600x400";

                _unitOfWork.Villa.Add(model);
                _unitOfWork.Save();

                TempData["success"] = "The villa has been created successfully";

                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Update(int villaId)
        {
            var result = _unitOfWork.Villa.Get(u => u.Id == villaId);

            if (result == null)
                return RedirectToAction("Error", "Home");

            return View(result);
        }
        [HttpPost]
        public IActionResult Update(Villa model)
        {
            if (ModelState.IsValid && model.Id > default(int))
            {
                if (model.Image != null)
                {

                    string fileName = Convert.ToString(Guid.NewGuid()) + Path.GetExtension(model.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                    if (!string.IsNullOrEmpty(model.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, model.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
                    }

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);

                    model.Image.CopyTo(fileStream);
                    model.ImageUrl = @"\images\VillaImage\" + fileName;
                }
                else
                    model.ImageUrl = "https://placehold.co/600x400";

                _unitOfWork.Villa.Update(model);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Error", "Home");
        }
        public IActionResult Delete(int villaId)
        {
            var result = _unitOfWork.Villa.Get(u => u.Id == villaId);
            if (result == null)
                return RedirectToAction("Error", "Home");
            return View(result);
        }
        [HttpPost]
        public IActionResult Delete(Villa model)
        {
            var result = _unitOfWork.Villa.Get(u => u.Id == model.Id);
            if (result is not null)
            {
                if (!string.IsNullOrEmpty(model.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, model.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                _unitOfWork.Villa.Remove(result);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa could not be deleted";
            return RedirectToAction("Error", "Home");
        }


    }
}
