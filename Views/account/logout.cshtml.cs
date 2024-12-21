using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_Project.Views.account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Session temizle
            HttpContext.Session.Clear();

            // Ana sayfaya y�nlendirme
            return RedirectToPage("/Home/Index");
        }
    }
}
