namespace RestaurantManagementApplication.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
