namespace RestaurantManagementApplication.DTO
{
    public class BillDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int BookingId { get; set; }
        //public Dictionary<int, string> Items { get; set; } = new Dictionary<int, string>();
        public string Items { get; set; }
        public decimal Amount { get; set; }
    }
}
