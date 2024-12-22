using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web_Project.Data;
using Web_Project.Models;

namespace Web_Project.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Submit(string name, string email, string message)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(message))
            {
                ViewBag.ErrorMessage = "Tüm alanların doldurulması zorunludur.";
                return View("Index");
            }

            var contactMessage = new ContactMessage
            {
                Name = name,
                Email = email,
                Message = message
            };

            _context.ContactMessages.Add(contactMessage);
            await _context.SaveChangesAsync();

            ViewBag.SuccessMessage = "Mesajınız başarıyla gönderildi! En kısa sürede sizinle iletişime geçeceğiz.";
            return View("Index");
        }
    }
}
