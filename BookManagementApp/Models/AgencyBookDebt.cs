using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookManagementApp.Models
{
    public class AgencyBookDebt
    {
        public int ID { get; set; }
        public int AgencyID { get; set; }
        public int BookID { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        public virtual Agency Agency { get; set; }
        public virtual Book Book { get; set; }
    }
}