using System;
using System.ComponentModel.DataAnnotations;

namespace RAPI2.Models
{
    public class Customer_Transaction
    {
        [Key]
        public int User_ID { get; set; }
        public int Transaction_ID { get; set; }
    }
}
