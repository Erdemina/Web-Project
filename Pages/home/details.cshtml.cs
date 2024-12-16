using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Project.Data;
using Web_Project.Models;

namespace Web_Project.Pages.Home
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public Property Property { get; set; }

        public IActionResult OnGet(int id)
        {
            // �lgili m�lk� bul
            Property = _context.Properties.FirstOrDefault(p => p.propid == id);

            if (Property == null)
            {
                return NotFound();
            }

            // Ziyaret kayd� ekle
            var viewRecord = new PropertyView
            {
                PropertyId = id,
                ViewedAt = DateTime.Now
            };

            _context.PropertyViews.Add(viewRecord);
            _context.SaveChanges();

            // Son 7 g�nl�k ziyaret say�s�n� hesapla
            var last7DaysViews = _context.PropertyViews
                .Where(v => v.PropertyId == id && v.ViewedAt >= DateTime.Now.AddDays(-7))
                .Count();

            ViewData["Last7DaysViews"] = last7DaysViews;

            return Page();
        }

    }
}
