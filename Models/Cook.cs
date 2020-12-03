using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Cook                               // Entity
    {   
        [Required][Key]
        public int User_ID{ get; set; }             // Unique Cook identifier
        public string Specialty { get; set;}        // Cook's Specialty
        public string Type { get; set; }            // The type of Cook (Part time or Full time)
    }
}
