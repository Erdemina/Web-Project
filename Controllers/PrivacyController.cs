using Microsoft.AspNetCore.Mvc;

namespace Web_Project.Controllers
{
    public class PrivacyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
