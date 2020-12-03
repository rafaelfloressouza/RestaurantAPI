using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Transaction                        // Entity
    {
        [Required][Key]
        public int Transaction_ID { get; set; }     // Unique transaction identifier
        public decimal Amount { get; set; }         // Money amount of transaction
        public DateTime Date_Time { get; set; }     // Date and time of the transaction
    }
}
