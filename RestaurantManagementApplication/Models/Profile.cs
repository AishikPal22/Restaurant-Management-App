namespace RestaurantManagementApplication.Models
{
    public class Profile
    {
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
