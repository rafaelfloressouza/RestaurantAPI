using System.ComponentModel.DataAnnotations;


namespace RestaurantAPI.Models
{
    public class Online_Order                       // Entity
    {
        [Required][Key]
        public int Order_ID { get; set; }           // Unique order identifier
        public string Application { get; set; }     // App from which the order was requested 
    }
}
