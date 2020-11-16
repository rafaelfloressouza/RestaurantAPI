using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Ingredient_Supplier
    {
        [Required]
        public string Supplier { get; set; }
        [Required]
        public string Ing_Name { get; set; }
    }
}
