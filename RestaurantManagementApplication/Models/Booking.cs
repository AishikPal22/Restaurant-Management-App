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

        //[Required(ErrorMessage = "Please select some items")]
        //public int ItemId { get; set; }
        //[JsonIgnore]
        //public Item Item { get; set; }

        //public int Quantity { get; set; } = 1;

        //public string BookingDate { get; set; } = DateTime.Now.ToString("D");


        //public decimal Amount { get; set; }
        //[ForeignKey("Bill")]
        //public int BillId { get; set; }
        //[JsonIgnore]
        //public Bill Bill { get; set; }
    }
}
