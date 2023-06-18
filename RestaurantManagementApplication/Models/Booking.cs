using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestaurantManagementApplication.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        //[ForeignKey("User")]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        public int PickupId { get; set; }
        [JsonIgnore]
        public Pickup Pickup { get; set; }

        public DateTime BookingDate { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
