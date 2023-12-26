using Microsoft.AspNetCore.Mvc;
using WhiteHotel.Application.Common.Interfaces;
using WhiteHotel.Web.ViewModels;

namespace WhiteHotel.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity"),
                Nights = 1,
                CheckInDate = DateOnly.FromDateTime(DateTime.Now)
            };
            return View(homeVM);
        }
        [HttpPost]
        public IActionResult Index(HomeVM homeVM)
        {
            homeVM.VillaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity");
            foreach (var villa in homeVM.VillaList)
            {
                if (villa.Id % 2 == 0)
                    villa.IsAvailable = false;
            }
            return View(homeVM);
        }
        public IActionResult GetVillasByDate(int nigths, DateOnly checkInDate)
        {
            var villaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity");

            foreach (var villa in villaList)
            {
                if (villa.Id % 2 == 0)
                    villa.IsAvailable = false;
            }

            HomeVM homeVM = new()
            {
                VillaList = villaList,               
                CheckInDate = checkInDate,
                Nights = nigths
            };
            return View(homeVM);
        }
        
        public IActionResult Error()
        {
            return View();
        }
    }
}
