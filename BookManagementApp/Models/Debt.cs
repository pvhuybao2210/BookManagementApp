using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookManagementApp.Models
{
    public class Debt
    {
        public int ID { get; set; }
        public int PublisherID { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Amount { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public virtual Publisher Publisher { get; set; }

    }
}