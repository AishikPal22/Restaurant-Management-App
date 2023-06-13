using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestaurantManagementApplication.Models
{
    public class Bill
    {
        //[Key]
        //public int Id { get; set; }

        ////[ForeignKey("Booking")]
        //public int BookingId { get; set; }
        //[JsonIgnore]
        //public Booking Booking { get; set; }

        //public string BookingDate { get; set; } = DateTime.Now.ToString("D");

        //public decimal Amount { get; set; }
        
        //public ICollection<Order> Orders { get; set; }
    }
}
