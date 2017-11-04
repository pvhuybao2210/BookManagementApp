using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookManagementApp.Models
{
    public class AgencyDebt
    {
        public int ID { get; set; }
        public int AgencyID { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Amount { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public virtual Agency Agency { get; set; }
    }
}