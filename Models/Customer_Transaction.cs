
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Models
{
    public class Customer_Transaction
    {
        [Key]
        [Column(Order=0)]
        public int User_ID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int Transaction_ID { get; set; }
    }
}
