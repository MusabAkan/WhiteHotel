using Microsoft.AspNetCore.Mvc;
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
    }
}
