using System;
using System.ComponentModel.DataAnnotations;


namespace RAPI2.Models
{
    public class Ingredient_Supplier
    {
        [Key]
        public string Supplier { get; set; }
        [Key]
        public string Ing_Name { get; set; }
    }
}
