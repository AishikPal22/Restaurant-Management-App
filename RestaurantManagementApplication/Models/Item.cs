using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementApplication.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter item name.")]
        [StringLength(450)]
        //add unique decalration in migration file
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter the price.")]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
