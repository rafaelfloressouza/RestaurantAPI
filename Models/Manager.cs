using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Manager                    // Entity
    {
        [Required][Key]
        public int User_ID { get; set; }    // Unique manager identifier
        public string Area { get; set; }    // Area of restaurant this person manages
    }
}
