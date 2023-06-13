using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementApplication.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter item name.")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter the price.")]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;

        public ICollection<Order> Orders { get; set; }
    }
}
