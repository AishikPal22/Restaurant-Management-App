using RestaurantManagementApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementApplication.DTO
{
    public class BookingDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string PickupName { get; set; }

        public string PickupAdress { get; set; }

        public string Date { get; set; }

        public BookingDTO(int id, string userName, string pickupName, string pickupAdress, string date)
        {
            Id = id;
            UserName = userName;
            PickupName = pickupName;
            PickupAdress = pickupAdress;
            Date = date;
        }
    }
}
