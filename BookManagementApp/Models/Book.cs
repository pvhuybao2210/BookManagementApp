using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookManagementApp.Models
{
    public class Book
    {
        public int ID { get; set; }
        public int PublisherID { get; set; }
        public int GenreID { get; set; }
        [Required(ErrorMessage = "Tên sách không được trống!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Tên tác giả không được trống!")]
        public string Author { get; set; }
        [Required(ErrorMessage = "Ngày xuất bản không được trống!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime PublicationDate{ get; set; }
        [Required(ErrorMessage = "Giá bán không được trống!")]
        [Range(0, int.MaxValue, ErrorMessage = "Giá bán không được âm")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int SellingPrice { get; set; }
        [Required(ErrorMessage = "Giá mua không được trống!")]
        [Range(0, int.MaxValue, ErrorMessage = "Giá mua không được âm")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int PurchasePrice { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual Publisher Publisher { get; set; }

    }
}