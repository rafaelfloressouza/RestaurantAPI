using System;
using System.ComponentModel.DataAnnotations;

namespace RAPI2.Models
{
    public class Waiter
    {
        [Key]
        public int User_ID { get; set;}
        public int Hours { get; set; }
        public string Type { get; set; }
    }
}
