using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Menu                               // Entity
    {
        [Required][Key]
        public string Type { get; set; }            // Unique menu identifier (ex: vegeterian, normal)
        public bool Available { get; set; }         // Availability of menu
    }
}
