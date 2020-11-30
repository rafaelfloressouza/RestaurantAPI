using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Cook
    {   
        [Key]
        public int User_ID{ get; set; }
        public string Specialty { get; set;}
        public string Type { get; set; }
    }
}
