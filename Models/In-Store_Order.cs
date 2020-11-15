using System;
using System.ComponentModel.DataAnnotations;

namespace RAPI2.Models
{
    public class In_Store_Order
    {
        [Key]
        public int Order_ID { get; set; }
        [Key]
        public int waiter_ID { get; set; }
    }
}
