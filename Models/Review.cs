using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Review
    {
        
        public int User_ID { get; set; }
        public int Review_ID { get; set;}
        public string Description { get; set; }
        public int Rating { get; set; }
        public int Dish_ID { get; set; }
    }
}
