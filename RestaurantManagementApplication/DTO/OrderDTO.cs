namespace RestaurantManagementApplication.DTO
{
    public class OrderDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } 
        public int BookingId { get; set; }
        public string BookingTime { get; set; }
        public int OrderId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public decimal ItemPrice { get; set; }
        public int ItemQuantity { get; set; }
    }
}
