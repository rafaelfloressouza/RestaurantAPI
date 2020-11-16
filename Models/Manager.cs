using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Manager
    {
        [Key]
        public int User_ID { get; set; }
        public string Area { get; set; }    
    }
}
