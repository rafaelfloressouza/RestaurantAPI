using System;
using System.ComponentModel.DataAnnotations;

namespace RAPI2.Models
{
    public class Ingredient
    {
        [Key]
        public string Name{ get; set; }
        public double Price { get; set; }
        public DateTime Exp_Date { get; set; }
        public double Quantity { get; set; }
    }
}
