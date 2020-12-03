using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Models
{
    public class Order_Dish                     // Many-to-Many relationship                  
    {
        [Required][Key][Column(Order=0)]
        public int Dish_ID { get; set; }        // Unique dish identifier
        [Required][Key][Column(Order=1)]
        public int Order_ID { get; set; }       // Unqiue order identifier
    }
}
