using System;
using System.ComponentModel.DataAnnotations;

namespace RAPI2.Models
{
    public class Order
    {
        [Key]
        public int Order_ID { get; set; }
        public int User_ID { get; set; }
        public DateTime Date_Time { get; set;}
    }
}
