using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookManagementApp.Models
{
    public class Book
    {
        public int ID { get; set; }
        [Required]
        public int PublisherID { get; set; }
        [Required]
        public int GenreID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime PublicationDate{ get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int SellingPrice { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int PurchasePrice { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual Publisher Publisher { get; set; }

    }
}