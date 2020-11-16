using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Order_Transaction
    {
        [Key]
        public int Transaction_ID { get; set; }
        [Key]
        public int Order_ID { get; set; }
    }
}
