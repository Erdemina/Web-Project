using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace YourNamespaceHere.Pages
{
    public class ContactModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Adýnýzý giriniz.")]
        public string Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "E-posta adresinizi giriniz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Mesajýnýzý yazýnýz.")]
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
                SuccessMessage = "Mesajýnýz baþarýyla gönderildi! En kýsa sürede size geri dönüþ yapýlacaktýr.";
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Mesaj gönderilirken bir hata oluþtu: " + ex.Message;
            }

            return Page();
        }

        private void SendEmail(string name, string email, string message)
        {
            string toAddress = "info@seyahat.com";
            string subject = "Yeni Ýletiþim Formu Mesajý";
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
