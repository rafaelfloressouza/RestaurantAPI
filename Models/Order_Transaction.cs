using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Order_Transaction
    {
        [Required]
        public int Transaction_ID { get; set; }
        [Required]
        public int Order_ID { get; set; }
    }
}
