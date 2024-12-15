using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Web_Project.Pages.houses
{
    public class ReservationModel : PageModel
    {
        [BindProperty]
        public ReservationFormModel ReservationData { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("/Reservation/Success");
        }

        public class ReservationFormModel
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public int Guests { get; set; }
            public List<string> BirthDates { get; set; }
            public string Address { get; set; }
        }
    }
}