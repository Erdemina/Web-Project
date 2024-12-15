using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web_Project.Data;
using Web_Project.Models;

namespace Web_Project.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(AppDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Property listesi
        public List<Property> Properties { get; set; }

        public void OnGet()
        {
            // Property bilgilerini ve Views iliþkisini dahil ederek çek
            Properties = _context.Properties
                .Include(p => p.Views) // Property ile Views iliþkisini getir
                .Take(3)               // Ýlk 3 kaydý al
                .ToList();
        }
    }
}
