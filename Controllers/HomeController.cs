using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Project.Data;
using Web_Project.Models;
using System;
using System.Linq;

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
            // Fetch all properties to display on the home page
            var properties = _context.Properties
                .Include(p => p.PropertyViews)
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

        // Search Functionality
        [HttpGet]
        public IActionResult Search(string destination, DateTime? checkIn, DateTime? checkOut, int? guests)
        {
            var properties = _context.Properties.AsQueryable();

            if (!string.IsNullOrEmpty(destination))
            {
                properties = properties.Where(p => p.location.Contains(destination));
            }

            if (checkIn.HasValue && checkIn.Value.Date >= DateTime.Today)
            {
                // Tarih geçmişte değilse filtre uygula (isteğe bağlı)
                properties = properties.Where(p => true); // Check-in kontrolü aktif değilse burayı değiştirin
            }
            else if (checkIn.HasValue)
            {
                ViewData["Error"] = "Check-in tarihi geçmişte olamaz.";
                return View("Index", new List<Property>()); // Hata durumunda boş sonuç döndür
            }

            if (checkOut.HasValue && checkOut.Value.Date >= DateTime.Today)
            {
                // Tarih geçmişte değilse filtre uygula (isteğe bağlı)
                properties = properties.Where(p => true); // Check-out kontrolü aktif değilse burayı değiştirin
            }
            else if (checkOut.HasValue)
            {
                ViewData["Error"] = "Check-out tarihi geçmişte olamaz.";
                return View("Index", new List<Property>()); // Hata durumunda boş sonuç döndür
            }

            if (guests.HasValue)
            {
                properties = properties.Where(p => p.guests >= guests);
            }

            return View("Index", properties.Include(p => p.PropertyViews).ToList());
        }

    }
}
