using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantManagementApplication.Models
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter a valid email id.")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        public string Password { get; set; }
        
        [RegularExpression("^[0-9]*$")]
        public string PhoneNo { get; set; }

        public int ProfileId { get; set; } = 3;
        [JsonIgnore]
        public Profile Profile { get; set; }
        
        public ICollection<Booking> Bookings { get; set; }

        public ICollection<Bill> Bills { get; set; }
    }
}
