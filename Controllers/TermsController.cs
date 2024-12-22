using Microsoft.AspNetCore.Mvc;

namespace Web_Project.Controllers
{
    public class TermsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
