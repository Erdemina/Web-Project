using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_Project.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Basit bir kontrol (sadece örnek)
            if (Email == "test@example.com" && Password == "1234")
            {
                return RedirectToPage("/Home/Index");
            }

            ModelState.AddModelError(string.Empty, "E-posta veya şifre hatalı.");
            return Page();
        }
    }
}
