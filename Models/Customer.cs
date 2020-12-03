using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Customer                       // Entity
    {
        [Required][Key]
        public int User_ID { get; set; }        // Unique Customer identifier
        public int? TableNo { get; set; }       // If customer is currently sitting at a table, this value containns the table number
    }
}
