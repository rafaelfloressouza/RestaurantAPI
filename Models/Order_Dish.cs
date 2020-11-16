using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Order_Dish
    {
        [Key]
        public int Dish_ID { get; set; }
        [Key]
        public int Order_ID { get; set; }
    }
}
