using Microsoft.AspNetCore.Mvc;

namespace Web_Project.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View(); // Views/About/Index.cshtml
        }
    }
}
