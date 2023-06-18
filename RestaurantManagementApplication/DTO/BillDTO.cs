namespace RestaurantManagementApplication.DTO
{
    public class BillDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int BookingId { get; set; }
        public string PickupName { get; set; }
        public string PickupLocation { get; set; }
        public string Orders { get; set; }
        public decimal Amount { get; set; }

        public BillDTO(int id, string name, int b_id, string p_name, string location, string item, decimal amount)
        {
            Id = id;
            UserName = name;
            BookingId = b_id;
            PickupName = p_name;
            PickupLocation = location;
            Orders = item;
            Amount = amount;
        }
    }
}
