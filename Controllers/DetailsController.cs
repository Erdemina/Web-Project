using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using Web_Project.Data;
using Web_Project.Models;

namespace Web_Project.Controllers
{
    public class DetailsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DetailsController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index(int id)
        {
            var property = _context.Properties.Find(id);
            if (property == null) return NotFound();

            var reviews = _context.Reviews
                .Include(r => r.User)
                .Where(r => r.PropertyId == id)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            var last7DaysViews = _context.PropertyViews
                .Count(v => v.PropertyId == id && v.ViewedAt >= DateTime.Now.AddDays(-7));

            property.ct_image = CountImagesInPropertyFolder(property.propid);

            var model = new DetailsViewModel
            {
                Property = property,
                Reviews = reviews,
                TotalReviews = reviews.Count,
                Last7DaysViews = last7DaysViews
            };

            AddViewRecord(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult AddReview(int propertyId, float rating, string comment)
        {
            // Kullanıcı oturumunu kontrol et
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail == null)
            {
                TempData["ErrorMessage"] = "Yorum yapmak için giriş yapmalısınız!";
                return RedirectToAction("Index", new { id = propertyId });
            }

            // Kullanıcıyı getir
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Geçersiz kullanıcı.";
                return RedirectToAction("Index", new { id = propertyId });
            }

            // Yeni yorum oluştur ve kaydet
            var newReview = new Reviews
            {
                PropertyId = propertyId,
                UserId = user.UserId,
                rating = rating,
                comment = comment,
                CreatedAt = DateTime.Now
            };

            _context.Reviews.Add(newReview);
            _context.SaveChanges();

            // Ortalama puanı güncelle
            UpdatePropertyRating(propertyId);

            TempData["SuccessMessage"] = "Yorumunuz başarıyla eklendi!";
            return RedirectToAction("Index", new { id = propertyId });
        }

        [HttpPost]
        public IActionResult AddBooking(int id, DateTime checkin, DateTime checkout, int guests)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                TempData["ErrorMessage"] = "Rezervasyon yapmak için giriş yapmalısınız!";
                return RedirectToAction("Login", "Account");
            }

            var userEmail = HttpContext.Session.GetString("UserEmail");
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null || checkin >= checkout)
            {
                TempData["ErrorMessage"] = "Geçerli bir tarih aralığı seçiniz.";
                return RedirectToAction("Index", new { id });
            }

            var property = _context.Properties.Find(id);
            if (property == null)
            {
                TempData["ErrorMessage"] = "Seçilen mülk bulunamadı.";
                return RedirectToAction("Index", new { id });
            }

            var totalPrice = (checkout - checkin).Days * (decimal)property.pricenight;

            var newBooking = new Booking
            {
                UserId = user.UserId,
                PropertyId = id,
                StartDate = checkin,
                EndDate = checkout,
                TotalPrice = totalPrice,
                BookingStatus = "Confirmed"
            };

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Rezervasyon başarıyla oluşturuldu!";
            return RedirectToAction("Index", new { id });
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
            var averageRating = _context.Reviews
                .Where(r => r.PropertyId == propertyId)
                .Average(r => r.rating);

            var roundedRating = Math.Round(averageRating, 1);

            var property = _context.Properties.Find(propertyId);
            if (property != null)
            {
                property.rating = (float)roundedRating;
                _context.Properties.Update(property);
                _context.SaveChanges();
            }
        }
    }

    public class DetailsViewModel
    {
        public Property Property { get; set; }
        public List<Reviews> Reviews { get; set; }
        public int TotalReviews { get; set; }
        public int Last7DaysViews { get; set; }
    }
}
