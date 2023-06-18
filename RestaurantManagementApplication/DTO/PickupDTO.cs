using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementApplication.DTO
{
    public class PickupDTO
    {
        public string Name { get; set; }
        
        public string ContactNo { get; set; }
        
        public string HighwayNo { get; set; }
        
        public string Location { get; set; }

        public PickupDTO(string name, string contact, string address, string location)
        {
            Name = name;
            ContactNo = contact;
            HighwayNo = address;
            Location = location;
        }
    }
}
