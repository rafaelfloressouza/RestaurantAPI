using System;
using System.ComponentModel.DataAnnotations;

namespace RAPI2.Models
{
    public class Dish_Ingredient
    {
        [Required]
        [Key]
        public int Dish_ID { get; set; }
        [Key]
        [Required]
        public int Ing_Name { get; set; }
    }
}
