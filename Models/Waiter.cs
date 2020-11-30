using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Waiter
    {
        [Required]
        [Key]
        public int User_ID { get; set;}
        public decimal Hours { get; set; }
        public string Type { get; set; }
    }
}
