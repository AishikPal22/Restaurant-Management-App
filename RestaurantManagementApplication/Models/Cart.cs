using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestaurantManagementApplication.Models
{
    public class Cart
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        [JsonIgnore]
        public Item Item { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
