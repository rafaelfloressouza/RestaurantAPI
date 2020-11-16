using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Employee
    {
        [Required]
        [Key]
        public int User_ID { get; set; }
        public DateTime Start_Date{get;set;}
        public string Job_Title { get; set; }
        public double Salary { get; set; }
        [Required]
        public int mgr_ID { get; set; }
    }
}
