﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteHotel.Application.Common.Utility;
using WhiteHotel.Application.Services.Interface;
using WhiteHotel.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class AmenityController : Controller
    {
        readonly IAmenityService _amenityService;
        readonly IVillaService _villaService;

        public AmenityController(IAmenityService amenityService, IVillaService villaService)
        {
            _amenityService = amenityService;
            _villaService = villaService;
        }
        public IActionResult Index()
        {
            var amenities = _amenityService.GetAllAmenities();
            return View(amenities);
        }
        public IActionResult Create()
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _villaService.GetAllVillas().Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                })
            };
            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {

            if (ModelState.IsValid)
            {
                _amenityService.CreateAmenity(obj.Amenity);
                TempData["success"] = "The amenity has been created successfully.";
                return RedirectToAction(nameof(Index));
            }

            obj.VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);
        }

        public IActionResult Update(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _villaService.GetAllVillas().Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                }),
                Amenity = _amenityService.GetAmenityById(amenityId)
            };
            if (amenityVM.Amenity == null)
            
                return RedirectToAction("Error", "Home");
            
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Update(AmenityVM amenityVM)
        {

            if (ModelState.IsValid)
            {
                _amenityService.UpdateAmenity(amenityVM.Amenity);
                TempData["success"] = "The amenity has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            amenityVM.VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(amenityVM);
        }

        public IActionResult Delete(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _villaService.GetAllVillas().Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                }),
                Amenity = _amenityService.GetAmenityById(amenityId)
            };
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }



        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            var result = _amenityService.GetAmenityById(amenityVM.Amenity.Id);
            if (result is not null)
            {
                _amenityService.DeleteAmenity(result.Id);
                TempData["success"] = "The amenity has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "The amenity could not be deleted";
            return View();
        }
    }


}