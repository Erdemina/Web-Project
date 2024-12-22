using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web_Project.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(); // Views/Contact/Index.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> Submit(string name, string email, string message)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(message))
            {
                ViewBag.ErrorMessage = "Tüm alanların doldurulması zorunludur.";
                return View("Index");
            }

            // Örnek: Mesajı işlemek veya bir servise göndermek
            await Task.Run(() => {
                // Örnek işlem: Loglama
                System.Diagnostics.Debug.WriteLine($"Yeni iletişim formu mesajı:\nAd: {name}\nE-posta: {email}\nMesaj: {message}");
            });

            // Başarılı bir işlem sonrası mesaj gönderimi
            ViewBag.SuccessMessage = "Mesajınız başarıyla gönderildi! En kısa sürede sizinle iletişime geçeceğiz.";
            return View("Index");
        }
    }
}
