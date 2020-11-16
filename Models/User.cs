using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class User
    {  
        [Key]
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } 
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string GivenName { get; set; }
        public string Addr1 { get; set; } 
        public string Addr2 { get; set; } 
        public string Province { get; set; } 
        public string PostalCode { get; set; } 
        public string Sex { get; set; } 
        public string Phone { get; set; } 
        public DateTime DOB { get; set; }
        public string Email { get; set; }   
    }
}
