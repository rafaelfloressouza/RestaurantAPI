using System;
using System.ComponentModel.DataAnnotations;


namespace RestaurantAPI.Models
{
    public class Online_Order
    {
        [Key]
        public int Order_ID { get; set; }
        public string Application { get; set; }       
    }
}
