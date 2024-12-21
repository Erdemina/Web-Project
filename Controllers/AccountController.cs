using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Web_Project.Data;
using Web_Project.Models; // Kullanıcı modeli için

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

        // Kişisel Bilgiler
        [HttpGet]
        public IActionResult PersonalInformation()
        {
            // Örnek kişisel bilgiler
            var personalInfo = new
            {
                FullName = "Ad Soyadı Çek",
                PreferredName = "Tercih edilen adı çek",
                Email = "E-posta adresini çek",
                Phone = "Telefon numarasını çek",
                Address = "Adresi al"
            };

            return View(personalInfo); // Views/Account/PersonalInformation.cshtml
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
    }
}
