using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Waiter
    {
        [Required]
        [Key]
        public int User_ID { get; set;}
        public double Hours { get; set; }
        public string Type { get; set; }
    }
}
