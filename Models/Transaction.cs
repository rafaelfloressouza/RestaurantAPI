using System;
using System.ComponentModel.DataAnnotations;

namespace RAPI2.Models
{
    public class Transaction
    {
        [Key]
        public int Transaction_ID { get; set; }
        public double Amount { get; set; }
        public DateTime Time { get; set; }
        public DateTime Date { get; set; }
    }
}
