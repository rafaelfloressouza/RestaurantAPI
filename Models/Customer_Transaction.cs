
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Models
{
    public class Customer_Transaction               // Many to Many Relationship
    {
        [Required][Key][Column(Order=0)]
        public int User_ID { get; set; }            // Unique Customer Identifier
        [Required][Key][Column(Order = 1)]
        public int Transaction_ID { get; set; }     // Unique Transaction Identifier
    }
}
