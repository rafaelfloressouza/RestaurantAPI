using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Table
    {
        [Required]
        [Key]
        public int TableNo { get; set; }
        public string Location { get; set; }
        public bool isOccupied { get; set; }
        [Required]
        public int waiter_ID { get; set; }
    }
}
