using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookManagementApp.Models
{
    public class Stock
    {
        public int ID { get; set; }
        public int BookID { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public virtual Book Book { get; set; }
    }
}