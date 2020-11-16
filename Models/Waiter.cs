using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Waiter
    {
        [Key]
        public int User_ID { get; set;}
        public int Hours { get; set; }
        public string Type { get; set; }
    }
}
