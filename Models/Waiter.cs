using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Waiter                         // Entity
    {
        [Required][Key]
        public int User_ID { get; set;}         // Unique waiter identifier
        public decimal Hours { get; set; }      // Hours worked per week
        public string Type { get; set; }        // Full time or part time
    }
}
