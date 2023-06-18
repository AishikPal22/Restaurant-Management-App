using RestaurantManagementApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementApplication.DTO
{
    public class MenuDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public MenuDTO(string name, string desc, decimal price)
        {
            Name = name;
            Description = desc;
            Price = price;
        }
    }
}
