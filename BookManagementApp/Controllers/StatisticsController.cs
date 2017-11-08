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
    public class StatisticsController : Controller
    {
        // GET: Statistic
        private BookContext db = new BookContext();

        public ActionResult Index()
        {
            DateTime filterDate = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(Request.Form["date"]))
            {
                var tempDate = Request.Form["date"].ToString();
                TimeSpan time = new TimeSpan(23, 59, 59);
                filterDate = DateTime.Parse(tempDate).Add(time);
            }

            int totalDebt = 0;
            int totalAgencyPay = 0;

            List<Publisher> publishers = db.Publishers.ToList();
            List<Debt> debts = db.Debts.ToList();
            List<Debt> debtResult = new List<Debt>();

            foreach (var x in publishers)
            {
                 Debt debt = debts
                .Where(s => s.PublisherID == x.ID &&
                (filterDate - s.Date).TotalMilliseconds >= 0)
                .OrderByDescending(s => s.Date)
                .FirstOrDefault();

                totalDebt += debt.Amount;

                debtResult.Add(debt);
            }
            ViewBag.publishers = publishers;
            ViewBag.totalDebt = totalDebt;

            List<AgencyReport> agencyReports = db.AgencyReports
                                                .Where(s => DbFunctions.TruncateTime(s.Date) <= filterDate)
                                                .Include(s => s.AgencyReportDetails)
                                                .ToList();
            int totalSellingPrice = 0;
            int totalPurchasePrice = 0;
            foreach (var x in agencyReports)
            {
                totalAgencyPay += x.Total;

                List<AgencyReportDetail> agencyReportDetails = db.AgencyReportDetails
                                                .Where(s => s.AgencyReportID == x.ID)
                                                .Include(s => s.Book)
                                                .ToList();
                foreach (AgencyReportDetail y in agencyReportDetails)
                {
                    totalSellingPrice += (y.Book.SellingPrice * y.Quantity);
                    totalPurchasePrice += (y.Book.PurchasePrice * y.Quantity);
                }

                ViewBag.totalAgencyPay = totalAgencyPay;
                ViewBag.tienLoi = totalSellingPrice - totalPurchasePrice;

                
            }
            return View(debtResult);
        }
    }
}