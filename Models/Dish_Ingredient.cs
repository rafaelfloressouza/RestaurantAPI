using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Dish_Ingredient
    {
        [Required]
        public int Dish_ID { get; set; }
        [Required]
        public string Ing_Name { get; set; }
    }
}
