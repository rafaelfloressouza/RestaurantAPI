using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Ingredient                         // Entity
    {
        [Required][Key]
        public string Name{ get; set; }             // Unique Ingredient identifier
        public decimal Price { get; set; }          // Price of the ingredient
        public DateTime Exp_Date { get; set; }      // Experation date of the ingredient
        public decimal Quantity { get; set; }       // Quantity of ingredient in KG
    }
}
