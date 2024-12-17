using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_Project.Pages.adminpage
{
    public class IndexModel : PageModel
    {
        public bool IsAdmin { get; private set; }

 
        public IActionResult OnGet()
        {

            // Session'dan UserRole deðerini al
            var roleString = HttpContext.Session.GetString("UserRole");

            // UserRole enum'una dönüþtür ve kontrol et
            if (Enum.TryParse(roleString, out UserRole role) && role == UserRole.Admin)
            {
                IsAdmin = true; // Kullanýcý admin
            }
            else
            {
                IsAdmin = false; // Kullanýcý admin deðil
            }

            // Admin olmayan kullanýcýlarý yönlendirme
            if (!IsAdmin)
            {
                return RedirectToPage("/Home/Index");
            }

            return Page();
        }
    }
}
