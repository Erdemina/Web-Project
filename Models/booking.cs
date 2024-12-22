using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Project.Models
{
    public class Booking
    {

        [Column("booking_id")]
        public int BookingId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("property_id")]
        public int PropertyId { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("total_price")]
        public decimal TotalPrice { get; set; }

        [Column("booking_status")]
        public string BookingStatus { get; set; }

        // İlgili mülk bilgileri için navigation property
        public Property Property { get; set; }
        public User User { get; set; }
    }
}
