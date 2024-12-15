using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        // Property tablosundan çekilecek liste
        public List<Property> Properties { get; set; }

        public void OnGet()
        {
            // Veritabanýndan Property bilgilerini çek
            Properties = _context.Properties.Take(3).ToList();
        }
    }
}
