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
    public class DebtsController : Controller
    {
        private BookContext db = new BookContext();

        public ActionResult Index()
        {
            ViewBag.publishers = new SelectList(db.Publishers, "ID", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Index(int publisherID)
        {
            ViewBag.publishers = new SelectList(db.Publishers, "ID", "Name");

            // borrow AgencyBookDebt model to store books debt
            List<AgencyBookDebt> bookDebts = new List<AgencyBookDebt>();

            if (!string.IsNullOrWhiteSpace(Request.Form["publisherID"]))
            {
                publisherID = Convert.ToInt32(Request.Form["publisherID"]);

                DateTime filterDate = DateTime.Now;

                if (!string.IsNullOrWhiteSpace(Request.Form["date"]))
                {
                    var tempDate = Request.Form["date"].ToString();
                    TimeSpan time = new TimeSpan(23, 59, 59);
                    filterDate = DateTime.Parse(tempDate).Add(time);
                }
                
                int totalDebt = 0;
                int totalAgencyPay = 0;

                List<Receipt> receipts = db.Receipts
                                                .Where(s => DbFunctions.TruncateTime(s.Date) <= filterDate
                                                    && s.PublisherID == publisherID
                                                )
                                                .Include(s => s.ReceiptDetails)
                                                .ToList();
                List<AgencyReport> agencyReports = db.AgencyReports
                                                .Where(s => DbFunctions.TruncateTime(s.Date) <= filterDate)
                                                .Include(s=>s.AgencyReportDetails)
                                                .ToList();

                foreach (var x in receipts)
                {
                    totalDebt += x.Total;

                    List<ReceiptDetail> receiptDetails = db.ReceiptDetails
                                                    .Where(s => s.ReceiptID == x.ID)
                                                    .Include(s => s.Book)
                                                    .ToList();
                    //return Json(receiptDetails, JsonRequestBehavior.AllowGet);
                    foreach (ReceiptDetail y in receiptDetails)
                    {
                        var bookDebt = bookDebts.Find(s => s.BookID == y.Book.ID);

                        if (bookDebt != null)
                        {
                            // if book exists, increase quantity
                            bookDebts.Find(s => s.BookID == y.Book.ID).Quantity += y.Quantity;
                        }
                        else
                        {
                            AgencyBookDebt a = new AgencyBookDebt()
                            {
                                BookID = y.BookID,
                                Quantity = y.Quantity,
                                Book = y.Book
                            };
                            bookDebts.Add(a);
                        }
                    }

                }

                foreach(AgencyReport x in agencyReports)
                {
                    foreach(AgencyReportDetail y in x.AgencyReportDetails)
                    {
                        if(y.Book.PublisherID == publisherID)
                        {
                            totalAgencyPay += (y.Quantity * y.UnitPrice);
                        }
                    }
                }

                ViewBag.totalDebt = totalDebt;
                ViewBag.totalAgencyPay = totalAgencyPay;
                ViewBag.date = filterDate;

            }

            return View(bookDebts);
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
