using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Customer
    {
        [Key]
        public int User_ID { get; set; }
        public int? TableNo { get; set; }
    }
}
