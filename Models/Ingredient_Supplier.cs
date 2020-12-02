using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Models
{
    public class Ingredient_Supplier
    {
        [Required]
        [Key][Column(Order=0)]
        public string Supplier { get; set; }
        [Required]
        [Key][Column(Order=1)]
        public string Ing_Name { get; set; }
    }
}
