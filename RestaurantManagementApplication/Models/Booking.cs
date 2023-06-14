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

        [Required(ErrorMessage = "Please enter a time for your booking (dd/MM/yyyy HH:mm).")]
        public string BookingTime { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
