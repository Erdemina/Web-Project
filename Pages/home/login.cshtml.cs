using Microsoft.AspNetCore.Http; // Session için gerekli
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;
using Web_Project.Data;
using Web_Project.models;

namespace Web_Project.Pages.home
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;

        public LoginModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kullanıcıyı email ile veritabanında ara
            var user = _context.Users.FirstOrDefault(u => u.Email == Email);

            if (user == null)
            {
                ErrorMessage = "Geçersiz e-posta veya şifre.";
                return Page();
            }

            // Şifreyi doğrula
            if (!VerifyPassword(Password, user.PasswordHash))
            {
                ErrorMessage = "Geçersiz e-posta veya şifre.";
                return Page();
            }

            // Kullanıcı oturum bilgilerini sakla
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("Username", user.Username);

            // Giriş başarılı
            return RedirectToPage("/Home/Index"); // Başarılı giriş sonrası yönlendirme
        }

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
