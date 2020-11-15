using System;
using System.ComponentModel.DataAnnotations;


namespace RAPI2.Models
{
    public class Online_Order
    {
        [Key]
        public int Order_ID { get; set; }
        public string Application { get; set; }       
    }
}
