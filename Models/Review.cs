using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Models
{
    public class Review
    {

        [Key][Column(Order=0)]
        public int User_ID { get; set; }
        [Key][Column(Order = 1)]
        public int Review_ID { get; set;}
        public string Description { get; set; }
        public int Rating { get; set; }
        public int? Dish_ID { get; set; }
    }
}
