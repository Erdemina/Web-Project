using Microsoft.AspNetCore.Mvc;
using Web_Project.Data;
using Web_Project.models;
using System.Security.Cryptography;
using System.Text;

namespace Web_Project.Controllers
{
    public class RegisterController : Controller
    {
        private readonly AppDbContext _context;

        public RegisterController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(); // Register formunu göstermek için
        }

        [HttpPost]
        public IActionResult Register(User user, string password)
        {
            if (ModelState.IsValid)
            {
                // Şifreyi hashle
                user.PasswordHash = HashPassword(password);
                user.CreatedAt = DateTime.Now;
                user.Role = "user"; // Varsayılan rol

                // Kullanıcıyı veritabanına ekle
                _context.Users.Add(user);
                _context.SaveChanges();

                // Başarılı kayıt sonrası yönlendirme
                return RedirectToAction("Success");
            }

            return View("Index"); // Hata varsa formu yeniden göster
        }

        public IActionResult Success()
        {
            return View(); // Başarılı kayıt sayfası
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
