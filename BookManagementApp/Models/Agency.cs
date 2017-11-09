using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookManagementApp.Models
{
    public class Agency
    {
        public Agency()
        {
            this.Invoices = new HashSet<Invoice>();
            this.AgencyReports = new HashSet<AgencyReport>();
        }
        public int ID { get; set; }
        [Required(ErrorMessage = "Tên không được trống")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string AccountNumber { get; set; }
       
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<AgencyReport> AgencyReports { get; set; }
    }
}