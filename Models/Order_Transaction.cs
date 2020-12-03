using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Models
{
    public class Order_Transaction
    {
        [Required]
        [Key][Column(Order=0)]
        public int Transaction_ID { get; set; }
        [Required]
        [Key][Column(Order=1)]
        public int Order_ID { get; set; }
    }
}
