using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
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
            // if user doesn't choose filter date, set default date
            DateTime fromDate = new DateTime(1995,1,1);
            DateTime toDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(Request.Form["toDate"]))
            {
                var tempDate = Request.Form["toDate"].ToString();
                TimeSpan time = new TimeSpan(23, 59, 59);
                toDate = DateTime.Parse(tempDate).Add(time);
                // add hour and minute to the end of date
            }
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;

            // get total payments from agencies => revenue
            int totalPayment = 0;
            ViewBag.totalAgencyPayment = 0;
            ViewBag.revenue = 0;

            List<Agency> agencies = db.Agencies.ToList();
            ViewBag.agencies = agencies;
   
            int[] paymentList = new int[100]; int count = 0;
            int totalSellingPrice = 0;
            int totalPurchasePrice = 0;

            foreach (Agency agency in agencies)
            {
                List<AgencyReport> agencyReports = db.AgencyReports
                                            .Where(
                                                s => s.AgencyID == agency.ID
                                                && DbFunctions.TruncateTime(s.Date) >= fromDate
                                                && DbFunctions.TruncateTime(s.Date) <= toDate)
                                            .Include(s => s.AgencyReportDetails)
                                            .ToList();
                if(agencyReports != null)
                {
                    foreach (var x in agencyReports)
                    {
                        paymentList[count] += x.Total;

                        totalPayment += x.Total;

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
                }
                
                count++;
            }

            ViewBag.paymentList = paymentList;
            ViewBag.revenue = totalSellingPrice - totalPurchasePrice;
            ViewBag.totalPayment = totalPayment;

            return View(paymentList);
        }


    }
}