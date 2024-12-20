using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Web_Project.Data;
using Web_Project.Models;

namespace Web_Project.Pages.details
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DetailsModel(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public Property Property { get; set; }
        public List<Reviews> Reviews { get; set; }
        public int TotalReviews { get; set; }
        public int Last7DaysViews { get; set; }

        [BindProperty]
        public string Comment { get; set; }

        [BindProperty]
        public float Rating { get; set; }

        public IActionResult OnGet(int id)
        {
            if (!LoadPageData(id)) return NotFound();
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            if (!HttpContext.Session.Keys.Contains("UserEmail"))
            {
                TempData["ErrorMessage"] = "Yorum yapabilmek i�in giri� yapmal�s�n�z!";
                return RedirectToPage("/home/login");
            }

            var userEmail = HttpContext.Session.GetString("UserEmail");
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user != null && !string.IsNullOrWhiteSpace(Comment))
            {
                var newReview = new Reviews
                {
                    UserId = user.UserId,
                    PropertyId = id,
                    rating = Rating,
                    comment = Comment,
                    CreatedAt = DateTime.Now
                };

                _context.Reviews.Add(newReview);
                _context.SaveChanges();

                // Ortalama rating'i g�ncelle
                UpdatePropertyRating(id);
            }

            if (!LoadPageData(id)) return NotFound();
            return Page();
        }

        private bool LoadPageData(int id)
        {
            Property = _context.Properties.Find(id);
            if (Property == null) return false;

            Reviews = _context.Reviews
                .Include(r => r.User)
                .Where(r => r.PropertyId == id)
                .ToList();

            TotalReviews = Reviews.Count;
            Last7DaysViews = CalculateLast7DaysViews(id);
            Property.ct_image = CountImagesInPropertyFolder(Property.propid);

            _context.SaveChanges();
            return true;
        }

        private void AddViewRecord(int propertyId)
        {
            var viewRecord = new PropertyView
            {
                PropertyId = propertyId,
                ViewedAt = DateTime.Now
            };

            _context.PropertyViews.Add(viewRecord);
            _context.SaveChanges();
        }

        private int CalculateLast7DaysViews(int propertyId)
        {
            return _context.PropertyViews
                .Count(v => v.PropertyId == propertyId && v.ViewedAt >= DateTime.Now.AddDays(-7));
        }

        private int CountImagesInPropertyFolder(int propertyId)
        {
            var folderPath = Path.Combine(_environment.WebRootPath, $"media/houses/h_{propertyId}");

            if (Directory.Exists(folderPath))
            {
                return Directory.GetFiles(folderPath, "*.jpeg").Length;
            }

            return 0;
        }

        private void UpdatePropertyRating(int propertyId)
        {
            // Ortalama rating'i hesapla ve yuvarla (2,1 format�)
            var averageRating = _context.Reviews
                .Where(r => r.PropertyId == propertyId)
                .Average(r => r.rating);

            var roundedRating = Math.Round(averageRating, 1); // 2,1 format�nda yuvarlama

            // Property'yi bul ve rating'i g�ncelle
            var property = _context.Properties.Find(propertyId);
            if (property != null)
            {
                property.rating = (float)roundedRating; // Yuvarlanm�� de�eri at�yoruz
                _context.Properties.Update(property);
                _context.SaveChanges();
            }
        }

    }
}
