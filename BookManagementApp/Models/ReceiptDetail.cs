using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManagementApp.Models
{
    public class ReceiptDetail
    {
        [Key, Column(Order = 0)]
        public int ReceiptID { get; set; }
        [Key, Column(Order = 1)]
        public int BookID { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int UnitPrice { get; set; }
        public virtual Book Book { get; set; }
    }
}