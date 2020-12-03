using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Employee                           // Entitiy
    {
        [Required][Key]
        public int User_ID { get; set; }            // Unique identifier of employee
        public DateTime Start_Date{get;set;}        // Working start date
        public string Job_Title { get; set; }       // Job title of the employee (ex: Cook, Manager)
        public decimal Salary { get; set; }         // Salary of the employee
        public int? mgr_ID { get; set; }            // Unique indifier of this employee's mananger
                                                    // Could be null until this one is assigned
    }
}
