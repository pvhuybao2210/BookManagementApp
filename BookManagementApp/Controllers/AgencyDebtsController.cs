using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookManagementApp.DAL;
using BookManagementApp.Models;

namespace BookManagementApp.Controllers
{
    public class AgencyDebtsController : Controller
    {
        private BookContext db = new BookContext();

        public ActionResult Index()
        {
            ViewBag.agencies = new SelectList(db.Agencies, "ID", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Index(int agencyID)
        {
            ViewBag.agencies = new SelectList(db.Agencies, "ID", "Name");

            List<AgencyBookDebt> agencyBookDebts = new List<AgencyBookDebt>();

            if (!string.IsNullOrWhiteSpace(Request.Form["agencyID"]))
            {
                agencyID = Convert.ToInt32(Request.Form["agencyID"]);

                if (!string.IsNullOrWhiteSpace(Request.Form["date"]))
                {
                    var tempDate = Request.Form["date"].ToString();
                    TimeSpan time = new TimeSpan(23, 59, 59);
                    DateTime date = DateTime.Parse(tempDate).Add(time);

                    // get agency's total debt on specific date
                    AgencyDebt agencyDebt = db.AgencyDebts
                                            .Where(s => s.AgencyID == agencyID
                                            && DbFunctions.TruncateTime(s.Date) <= date)
                                            .OrderByDescending(s => s.Date)
                                            .FirstOrDefault();
                    ViewBag.total = agencyDebt.Amount;

                    // get all agency's invoices from start to specific date
                    List<Invoice> invoices = db.Invoices
                                            .Where(s => s.AgencyID == agencyID
                                            && DbFunctions.TruncateTime(s.Date) <= date)
                                            .Include(s=>s.InvoiceDetails)
                                            .ToList();

                    foreach (var x in invoices)
                    {
                        List<InvoiceDetail> invoiceDetails = db.InvoiceDetails
                                                        .Where(s => s.InvoiceID == x.ID)
                                                        .Include(s => s.Book)
                                                        .ToList();
                        foreach (var y in invoiceDetails)
                        {
                            var agencyBookDebt = agencyBookDebts.Find(s => s.BookID == y.BookID);

                            if (agencyBookDebt != null)
                            {
                                // if book exists, increase quantity
                                agencyBookDebts.Find(s => s.BookID == y.BookID).Quantity += y.Quantity;
                            } 
                            else
                            {
                                AgencyBookDebt a = new AgencyBookDebt()
                                {
                                    BookID = y.BookID,
                                    Quantity = y.Quantity,
                                    Book = y.Book
                                };
                                agencyBookDebts.Add(a);
                            }
                        }

                    }

                    // get all agency's reports from start to specific date
                    List<AgencyReport> agencyReports = db.AgencyReports
                                            .Where(s => DbFunctions.TruncateTime(s.Date) <= date)
                                            .ToList();
                    foreach (var x in agencyReports)
                    {
                        List<AgencyReportDetail> agencyReportDetails = db.AgencyReportDetails
                                                        .Where(s => s.AgencyReportID == x.ID)
                                                        .Include(s => s.Book)
                                                        .ToList();
                        foreach (var y in agencyReportDetails)
                        {
                            foreach (var z in agencyBookDebts)
                            {
                                if (z.BookID == y.BookID)
                                {
                                    z.Quantity -= y.Quantity;
                                }
                            }

                        }

                    }
                    ViewBag.date = date;
                }
                else
                {
                    agencyBookDebts = db.AgencyBookDebts
                                                    .Where(s => s.AgencyID == agencyID)
                                                    .Include(s => s.Book)
                                                    .ToList();

                    int total = 0;

                    foreach (var i in agencyBookDebts)
                    {
                        total += (i.Quantity * i.Book.SellingPrice);
                    }
                    ViewBag.total = total;

                    ViewBag.date = "Hôm nay";
                }

            }

            return View(agencyBookDebts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
