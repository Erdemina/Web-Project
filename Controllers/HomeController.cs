using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Project.Data;
using Web_Project.Models;

namespace Web_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // Home Page
        public IActionResult Index()
        {
            // Fetch top 3 properties and include their related Views
            var properties = _context.Properties
                .Include(p => p.Views)
                .Take(3)
                .ToList();

            return View(properties); // Pass the properties to the view
        }

        // About Page
        public IActionResult About()
        {
            ViewData["Title"] = "About Us";
            ViewData["Description"] = "Learn more about our mission and team.";

            return View(); // Simply return the view
        }

        // Contact Page
        public IActionResult Contact()
        {
            ViewData["Title"] = "Contact Us";
            ViewData["Description"] = "Feel free to reach out to us with any questions.";

            return View(); // Simply return the view
        }
    }
}
