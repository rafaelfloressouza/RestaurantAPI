using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Dish                               // Entity
    {
        [Required][Key]
        public int Dish_ID { get; set; }            // Unique dish identifier 
        public bool Available { get; set; }         // Determines is dish is avaialable
        public Decimal Price { get; set; }          // Price of the dish
        public string Description { get; set; }     // Brief descirption of what the dish is
        //(One to Many Relationship)
        public string Menu_Type { get; set; }       // Type of menu where the dish belongs (ex: vegetarian, normal)
    }
}
