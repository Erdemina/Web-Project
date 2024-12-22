using Microsoft.AspNetCore.Mvc;

namespace Web_Project.Controllers
{
    public class ReservationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
