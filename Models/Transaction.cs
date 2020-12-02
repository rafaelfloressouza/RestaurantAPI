using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Transaction
    {
        [Key]
        public int Transaction_ID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date_Time { get; set; }
    }
}
