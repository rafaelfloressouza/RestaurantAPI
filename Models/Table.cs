using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Table                              // Entity
    {
        [Required][Key]
        public int TableNo { get; set; }            // Unique table identifier
        public string Location { get; set; }        // Tabble location inside the restaurant
        public bool isOccupied { get; set; }        // Wheter tale is free
        [Required]
        public int waiter_ID { get; set; }          // Unique waiter identifer who is assigned the table
    }
}
