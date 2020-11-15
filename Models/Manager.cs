using System;
using System.ComponentModel.DataAnnotations;

namespace RAPI2.Models
{
    public class Manager
    {
        [Key]
        public int User_ID { get; set; }
        public string Area { get; set; }    
    }
}
