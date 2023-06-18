using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestaurantManagementApplication.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int BookingId { get; set; }
        [JsonIgnore]
        public Booking Booking { get; set; }

        [Required(ErrorMessage = "Please order an item")]
        public string ItemName { get; set; }
        public int ItemId { get; set; } 
        [JsonIgnore]
        public Item Item { get; set; }

        public int Quantity { get; set; } = 1;

        public decimal Price { get; set; }
    }
}
