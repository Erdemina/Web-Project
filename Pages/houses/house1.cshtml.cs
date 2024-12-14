using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Project.models; // Model dosyanızın namespace'i

namespace Web_Project.Pages.Houses
{
    public class House1Model : PageModel
    {
        // House modelini kullanarak detayları tanımlayın
        public House HouseDetails { get; set; }

        public void OnGet()
        {
            // Dinamik olarak House detaylarını doldurun
            HouseDetails = new House
            {
                Id = 1,
                Name = "Havuzlu Müstakil Villa",
                Price = 8000,
                Features = new List<string>
                {
                    "Bahçe Manzarası",
                    "Wifi",
                    "Dağ Manzarası",
                    "Ücretsiz Otopark"
                },
                Images = new List<HouseImage>
                {
                    new HouseImage { Url = "/images/a.jpg" },
                    new HouseImage { Url = "/images/b.jpg" },
                    new HouseImage { Url = "/images/c.jpg" }
                }
            };
        }
    }
}
