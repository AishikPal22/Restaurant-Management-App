namespace RestaurantManagementApplication.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public int ItemQuantity { get; set; }
        public decimal OrderAmount { get; set; }

        public OrderDTO(int id, string name, decimal price, int qty, decimal amount)
        {
            OrderId = id;
            ItemName = name;
            ItemPrice = price;
            ItemQuantity = qty;
            OrderAmount = amount;
        }
    }
}
