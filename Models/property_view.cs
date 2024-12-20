using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Project.Models
{
    [Table("PropertyViews")]
    public class PropertyView
    {
        [Key]
        [Column("view_id")]
        public int ViewId { get; set; } // Primary Key

        [Column("property_id")]
        public int PropertyId { get; set; } // Foreign Key (Hangi mülk)

        [Column("viewed_at")]
        public DateTime ViewedAt { get; set; } = DateTime.Now; // Ziyaret zamanı

        [ForeignKey("PropertyId")]
        public Property Property { get; set; } // Navigation Property
    }
}
