using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace YourNamespaceHere.Pages
{
    public class ContactModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Ad�n�z� giriniz.")]
        public string Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "E-posta adresinizi giriniz.")]
        [EmailAddress(ErrorMessage = "Ge�erli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Mesaj�n�z� yaz�n�z.")]
        public string Message { get; set; }

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                SendEmail(Name, Email, Message);
                SuccessMessage = "Mesaj�n�z ba�ar�yla g�nderildi! En k�sa s�rede size geri d�n�� yap�lacakt�r.";
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Mesaj g�nderilirken bir hata olu�tu: " + ex.Message;
            }

            return Page();
        }

        private void SendEmail(string name, string email, string message)
        {
            string toAddress = "info@seyahat.com";
            string subject = "Yeni �leti�im Formu Mesaj�";
            string body = $"Ad: {name}\nE-posta: {email}\nMesaj:\n{message}";

            using (var smtp = new SmtpClient("smtp.your-email-provider.com"))
            {
                smtp.Port = 587; 
                smtp.Credentials = new System.Net.NetworkCredential("your-email@example.com", "your-email-password");
                smtp.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("your-email@example.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(toAddress);
                smtp.Send(mailMessage);
            }
        }
    }
}
