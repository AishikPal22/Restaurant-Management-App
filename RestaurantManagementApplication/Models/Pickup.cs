using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementApplication.Models
{
    public class Pickup
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Please enter a valid name.")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Please enter a valid contact number.")]
        [RegularExpression("^[0-9]*$")]
        public string ContactNo { get; set; }
        
        [Required(ErrorMessage = "Please enter a valid highway number.")]
        public string HighwayNo { get; set; }
        
        [Required(ErrorMessage = "Please enter a valid city/area name.")]
        [StringLength(450)]
        //add unique decalration in migration file
        public string Location { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
