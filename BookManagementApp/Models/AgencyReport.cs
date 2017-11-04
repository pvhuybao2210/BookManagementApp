using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookManagementApp.Models
{
    public class AgencyReport
    {
        public AgencyReport()
        {
            this.AgencyReportDetails = new HashSet<AgencyReportDetail>();
        }
        public int ID { get; set; }
        public int AgencyID { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Total { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<AgencyReportDetail> AgencyReportDetails { get; set; }
        public virtual Agency Agency { get; set; }
    }
}