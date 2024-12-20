using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_Project.Pages.Reservation
{
    public class PaymentModel : PageModel
    {
        [BindProperty]
        public PaymentFormModel PaymentData { get; set; } = new PaymentFormModel();

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

        public class PaymentFormModel
        {
            public string CardNumber { get; set; } = string.Empty;
            public string CardHolder { get; set; } = string.Empty;
            public string ExpiryDate { get; set; } = string.Empty;
            public string CVV { get; set; } = string.Empty;
        }
    }
}
