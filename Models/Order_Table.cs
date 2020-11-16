using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Order_Table
    {
        [Required]
        public int Order_ID { get; set; }
        [Required]
        public int TableNo { get; set; }
    }
}
