﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestaurantManagementApplication.Models
{
    public class Bill
    {
        //[Required]
        public int Id { get; set; }
        
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        public int BookingId { get; set; }

        //public List<Order> AllOrders { get; set; }
        public string AllOrders { get; set; }

        public decimal Amount { get; set; } = 0;
    }
}
