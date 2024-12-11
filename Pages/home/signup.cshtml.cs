using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_Project.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Kaydolma işlemini burada gerçekleştirin (örneğin, veritabanına kaydedin)
            if (ModelState.IsValid)
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }
    }
}
