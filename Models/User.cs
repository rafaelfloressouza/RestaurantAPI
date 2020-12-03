using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class User                               //Entity
    {  
        [Required][Key]
        public int ID { get; set; }                 // Unique user identifier
        public string Username { get; set; }        // Username of user       
        public string Password { get; set; }        // Pass of user
        [Required]
        public string FirstName { get; set; }       // First name of user
        public string MiddleName { get; set; }      // Middle name of user (if applicable)
        [Required]
        public string LastName { get; set; }        // Last name of user
        public string GivenName { get; set; }       // Given name of user (if applicable)
        [Required]
        public string Addr1 { get; set; }           // Line one of adsress
        public string Addr2 { get; set; }           // Line two of address
        [Required]
        public string Province { get; set; }        // Province where user lives
        [Required]
        public string PostalCode { get; set; }      // Postal code of where user lives
        public string Sex { get; set; }             // Sex of user (if applicable)
        [Required]
        public string Phone { get; set; }           // Phone of user (with country code)
        [Required]
        public DateTime DOB { get; set; }           // Date of birth of user
        [Required]
        public string Email { get; set; }           // Email of user

    }
}