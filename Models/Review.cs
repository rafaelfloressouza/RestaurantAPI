using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Models
{
    public class Review                             // Weak Entity
    {

        [Required][Key][Column(Order=0)]
        public int User_ID { get; set; }            // Unique user identifier
        [Required][Key][Column(Order = 1)]
        public int Review_ID { get; set;}           // Unique review identifier
        public string Description { get; set; }     // Brief description  
        public int Rating { get; set; }             
        public int? Dish_ID { get; set; }           // Optional dish identifier
    }
}
