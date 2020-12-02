using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Models
{
    public class Order_Dish
    {
        [Required]
        [Key][Column(Order=0)]
        public int Dish_ID { get; set; }
        [Required]
        [Key][Column(Order=1)]
        public int Order_ID { get; set; }
    }
}
