using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class In_Store_Order                 // Entity
    {
        [Required][Key]
        public int Order_ID { get; set; }       // Unique order indentifier
        [Required]
        public int TableNo { get; set; }        // Unique table identifier
    }
}
