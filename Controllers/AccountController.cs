using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Web_Project.Data;
using Web_Project.Models; // Kullanıcı modeli için
using Microsoft.EntityFrameworkCore;


namespace Web_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // Login
        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Views/Account/Login.cshtml
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "E-posta ve şifre gereklidir.";
                return View();
            }

            // Kullanıcıyı email veya kullanıcı adı ile veritabanında ara
            var user = _context.Users.FirstOrDefault(u => u.Email == email || u.Username == email);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Geçersiz e-posta veya şifre.";
                return View();
            }

            // Şifreyi doğrula
            if (!VerifyPassword(password, user.PasswordHash))
            {
                ViewBag.ErrorMessage = "Geçersiz e-posta veya şifre.";
                return View();
            }

            // Kullanıcı oturum bilgilerini sakla
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());

            // Giriş başarılı
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string email, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Tüm alanlar gereklidir.";
                return View();
            }

            // Kullanıcı var mı kontrolü
            if (_context.Users.Any(u => u.Email == email || u.Username == username))
            {
                ViewBag.ErrorMessage = "Bu e-posta veya kullanıcı adı zaten alınmış.";
                return View();
            }

            // Şifreyi hashle
            var hashedPassword = HashPassword(password);

            // Yeni kullanıcıyı oluştur
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = hashedPassword,
                Role = UserRole.User, // Enum kullanımı ile varsayılan rol atanıyor
                CreatedAt = DateTime.Now
            };

            // Veritabanına ekle
            _context.Users.Add(user);
            _context.SaveChanges();

            // Başarılı işlem sonrası yönlendirme
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Session temizle
            HttpContext.Session.Clear();

            // Ana sayfaya yönlendirme
            return RedirectToAction("Index", "Home");
        }

        // Kişisel Bilgiler
        [HttpGet]
        public IActionResult PersonalInformation()
        {
            return View(PersonalInformation); // Views/Account/PersonalInformation.cshtml
        }

        // Şifre hashleme
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        // Şifre doğrulama
        private bool VerifyPassword(string password, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                var inputHash = Convert.ToBase64String(hash);

                return inputHash == storedHash;
            }
        }

        [HttpGet]
        public IActionResult Travels()
        {
            // Kullanıcı oturum bilgilerini al
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Kullanıcıyı veritabanından çek
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Kullanıcının rezervasyonlarını al
            var bookings = _context.Bookings
                .Where(b => b.UserId == user.UserId)
                .Include(b => b.Property) // İlgili mülk bilgilerini ekler
                .ToList();

            // Rezervasyon yoksa uyarı mesajı
            if (bookings == null || !bookings.Any())
            {
                ViewBag.Message = "Gelecek bir rezervasyonunuz bulunmamaktadır.";
            }

            return View(bookings); // View'e model olarak rezervasyonları gönder
        }


    }
}

