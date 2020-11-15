using System;
using System.ComponentModel.DataAnnotations;

namespace RAPI2.Models
{
    public class Menu
    {
        [Key]
        public string Type { get; set; }
        public bool Available { get; set; }
    }
}
