using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Customer_Transaction
    {
        [Key]
        public int User_ID { get; set; }
        public int Transaction_ID { get; set; }
    }
}
