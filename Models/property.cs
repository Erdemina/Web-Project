using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Project.Models
{
    public class Property
    {
        [Column("property_id")]
        public int propid { get; set; }

        [Column("owner_id")]
        public int ownerid { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public string location { get; set; }

        [Column("price_per_night")]
        public float pricenight { get; set; }

        public float rating { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // Navigation Property: PropertyViews ile ilişki
        public ICollection<PropertyView> Views { get; set; }

        // Computed Property: Son 7 gündeki ziyaret sayısı
        [NotMapped]
        public int ViewsLast7Days => Views?.Count(v => v.ViewedAt >= DateTime.Now.AddDays(-7)) ?? 0;
    }
}
