using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Web_Project.Pages.Reservation
{
    public class ReservationModel : PageModel
    {
        [BindProperty]
        public ReservationFormModel ReservationData { get; set; } = new ReservationFormModel();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("/Reservation/Payment");
        }

        public class ReservationFormModel
        {
            public string FullName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public int Guests { get; set; } = 1;
            public List<string> BirthDates { get; set; } = new List<string>();
            public string Address { get; set; } = string.Empty;
        }
    }
}
