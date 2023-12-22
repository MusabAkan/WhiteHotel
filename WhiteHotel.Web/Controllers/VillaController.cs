using Microsoft.AspNetCore.Mvc;
using WhiteHotel.Application.Common.Interfaces;
using WhiteHotel.Domain.Entities;

namespace WhiteHotel.Web.Controllers
{
    public class VillaController : Controller
    {
        readonly IVillaRepository _repository;

        public VillaController(IVillaRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            var result = _repository.GetAll();
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
                _repository.Add(model);
                _repository.Save();
                TempData["success"] = "The villa has been created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Update(int villaId)
        {
            var result = _repository.Get(u => u.Id == villaId);
            if (result == null)
                return RedirectToAction("Error", "Home");
            return View(result);
        }
        [HttpPost]
        public IActionResult Update(Villa model)
        {
            if (ModelState.IsValid && model.Id > default(int)) 
            {
                _repository.Update(model);
                _repository.Save();
                TempData["success"] = "The villa has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Error", "Home");
        }
        public IActionResult Delete(int villaId)
        {
            var result = _repository.Get(u => u.Id == villaId);
            if (result == null)
                return RedirectToAction("Error", "Home");
            return View(result);
        }
        [HttpPost]
        public IActionResult Delete(Villa model)
        {
            var result = _repository.Get(u => u.Id == model.Id);
            if (result is not null)
            {
                _repository.Remove(model);
                _repository.Save();
                TempData["success"] = "The villa has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa could not be deleted";
            return RedirectToAction("Error", "Home");
        }

     
    }
}
