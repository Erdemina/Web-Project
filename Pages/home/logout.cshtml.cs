using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_Project.Pages.home
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Session temizle
            HttpContext.Session.Clear();

            // Ana sayfaya yönlendirme
            return RedirectToPage("/Home/Index");
        }
    }
}
