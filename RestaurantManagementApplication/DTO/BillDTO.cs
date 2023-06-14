namespace RestaurantManagementApplication.DTO
{
    public class BillDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int BookingId { get; set; }
        public int OrderId { get; set; }
        public string ItemName { get; set; }
        public decimal Amount { get; set; }
    }
}
