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
    public class HomeController : Controller
    {
        private BookContext db = new BookContext();

        public ActionResult Index()
        {
            // if user doesn't choose filter date, get current date by default
            DateTime filterDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(Request.Form["date"]))
            {
                var tempDate = Request.Form["date"].ToString();
                TimeSpan time = new TimeSpan(23, 59, 59);
                filterDate = DateTime.Parse(tempDate).Add(time);
            }

            // get all debts from all publishers
            int totalDebt = 0;
            ViewBag.totalDebt = 0;

            List<Publisher> publishers = db.Publishers.ToList();
            ViewBag.publishers = publishers;

            List<Debt> debts = db.Debts.ToList();
            List<Debt> debtResult = new List<Debt>();

            if(debts != null)
            {
                foreach (var x in publishers)
                {
                    Debt debt = debts
                   .Where(s => s.PublisherID == x.ID &&
                   (filterDate - s.Date).TotalMilliseconds >= 0)
                   .OrderByDescending(s => s.Date)
                   .FirstOrDefault();

                    if(debt != null)
                    {
                        debtResult.Add(debt);
                        totalDebt += debt.Amount;
                    } 
                }
                ViewBag.totalDebt = totalDebt;
            }

            // get total payments from agencies => revenue
            int totalAgencyPayment = 0;
            ViewBag.totalAgencyPayment = 0;
            ViewBag.revenue = 0;

            List<AgencyReport> agencyReports = db.AgencyReports
                                                .Where(s => DbFunctions.TruncateTime(s.Date) <= filterDate)
                                                .Include(s => s.AgencyReportDetails)
                                                .ToList();
            int totalSellingPrice = 0;
            int totalPurchasePrice = 0;

            if(agencyReports != null)
            {
                foreach (var x in agencyReports)
                {
                    totalAgencyPayment += x.Total;

                    List<AgencyReportDetail> agencyReportDetails = db.AgencyReportDetails
                                                    .Where(s => s.AgencyReportID == x.ID)
                                                    .Include(s => s.Book)
                                                    .ToList();
                    foreach (AgencyReportDetail y in agencyReportDetails)
                    {
                        totalSellingPrice += (y.Book.SellingPrice * y.Quantity);
                        totalPurchasePrice += (y.Book.PurchasePrice * y.Quantity);
                    }
                }
                ViewBag.revenue = totalSellingPrice - totalPurchasePrice;
                ViewBag.totalAgencyPayment = totalAgencyPayment;
            }
            
            return View(debtResult);
        }


    }
}