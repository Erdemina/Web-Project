using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_Project.Pages.adminpage
{
    public class IndexModel : PageModel
    {
        public bool IsAdmin { get; private set; }

 
        public IActionResult OnGet()
        {

            // Session'dan UserRole de�erini al
            var roleString = HttpContext.Session.GetString("UserRole");

            // UserRole enum'una d�n��t�r ve kontrol et
            if (Enum.TryParse(roleString, out UserRole role) && role == UserRole.Admin)
            {
                IsAdmin = true; // Kullan�c� admin
            }
            else
            {
                IsAdmin = false; // Kullan�c� admin de�il
            }

            // Admin olmayan kullan�c�lar� y�nlendirme
            if (!IsAdmin)
            {
                return RedirectToPage("/Home/Index");
            }

            return Page();
        }
    }
}
