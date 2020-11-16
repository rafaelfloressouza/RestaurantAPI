using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Order_Dish
    {
        [Required]
        public int Dish_ID { get; set; }
        [Required]
        public int Order_ID { get; set; }
    }
}
