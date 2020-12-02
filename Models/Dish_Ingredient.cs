using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Models
{
    public class Dish_Ingredient
    {
        [Required]
        [Key][Column(Order=0)]
        public int Dish_ID { get; set; }
        [Key][Column(Order = 1)]
        public string Ing_Name { get; set; }
    }
}
