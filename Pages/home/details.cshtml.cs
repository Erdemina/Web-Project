using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Web_Project.Data;
using Web_Project.Models;

namespace Web_Project.Pages.Home
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
        public List<Reviews> Reviews { get; set; } // Tüm deðerlendirmeleri tutar


        public int TotalReviews { get; set; } // Toplam deðerlendirme sayýsýný tutacak

        public int Last7DaysViews { get; set; } // Son 7 günlük ziyaret sayýsý

        public IActionResult OnGet(int id)
        {
            // Ýlgili mülkü bul
            Property = _context.Properties.FirstOrDefault(p => p.propid == id);

            if (Property == null)
            {
                return NotFound();
            }

            // Deðerlendirmeleri ve kullanýcý bilgilerini yükle
            Reviews = _context.Reviews
                .Include(r => r.User) // User bilgilerini dahil et
                .Where(r => r.PropertyId == id)
                .ToList();
            TotalReviews = _context.Reviews.Count(r => r.PropertyId == id);
            // Ziyaret kaydý ekle
            AddViewRecord(id);

            // Son 7 günlük ziyaret sayýsýný hesapla
            Last7DaysViews = CalculateLast7DaysViews(id);

            // JPEG dosya sayýsýný kontrol et ve ct_image'e kaydet
            var imageCount = CountImagesInPropertyFolder(Property.propid);
            Property.ct_image = imageCount;

            // Deðeri veritabanýnda güncelle
            _context.Properties.Update(Property);
            _context.SaveChanges();

            return Page();
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
                .Where(v => v.PropertyId == propertyId && v.ViewedAt >= DateTime.Now.AddDays(-7))
                .Count();
        }

        private int CountImagesInPropertyFolder(int propertyId)
        {
            // Klasör yolunu oluþtur
            var folderPath = Path.Combine(_environment.WebRootPath, $"media/houses/h_{propertyId}");

            if (Directory.Exists(folderPath))
            {
                // Klasördeki JPEG dosyalarýnýn sayýsýný al
                return Directory.GetFiles(folderPath, "*.jpeg").Length;
            }

            // Klasör yoksa, 0 döndür
            return 0;
        }
    }
}
