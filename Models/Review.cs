using System;
using System.ComponentModel.DataAnnotations;

namespace RAPI2.Models
{
    public class Review
    {
        [Key]
        public int User_ID { get; set; }
        [Key]
        public int Review_ID { get; set;}
        public string Description { get; set; }
        public int Rating { get; set; }
        public int Dish_ID { get; set; }
    }
}
