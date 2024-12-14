using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;
using Web_Project.Data;
using Web_Project.models;

namespace Web_Project.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _context;

        public RegisterModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Şifreyi hashle
            var hashedPassword = HashPassword(Password);

            // Yeni kullanıcıyı oluştur
            var user = new User
            {
                Username = Username,
                Email = Email,
                PasswordHash = hashedPassword,
                Role = "user",
                CreatedAt = DateTime.Now
            };

            // Veritabanına ekle
            _context.Users.Add(user);
            _context.SaveChanges();

            // Başarılı işlem sonrası yönlendirme
            return RedirectToPage("/Success");
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
