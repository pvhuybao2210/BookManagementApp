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
    public class AgencyReportsController : Controller
    {
        private BookContext db = new BookContext();

        // GET: AgencyReports
        public ActionResult Index()
        {
            var agencyReport = db.AgencyReport.Include(a => a.Agency);
            return View(agencyReport.ToList());
        }

        // GET: AgencyReports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.agencyReportInfo = db.AgencyReport.Include(s => s.Agency)
                 .SingleOrDefault(x => x.ID == id);

            List<AgencyReportDetail> agencyReportDetails = db.AgencyReportDetails
                       .Where(s => s.AgencyReportID == id)
                       .Include(s => s.Book)
                       .ToList();
            
            return View(agencyReportDetails);
        }

        // GET: AgencyReports/Create
        public ActionResult Create()
        {
            ViewBag.agencies = new SelectList(db.Agencies, "ID", "Name");
            ViewBag.currentDate = DateTime.Now.ToString("dd/MM/yyyy h:mm:ss");

            return View();
        }

        [HttpPost]
        public ActionResult CreateAgencyReport()
        {
            AgencyReport agencyReport = new AgencyReport
            {
                AgencyID = Convert.ToInt32(Request.Form["agencyID"].ToString()),
                Total = 0,
                Date = DateTime.Now,
                Description = Request.Form["Description"].ToString(),
                Status = true
            };

            db.AgencyReport.Add(agencyReport);
            db.SaveChanges();

            Session["agencyID"] = agencyReport.AgencyID;
            Session["agencyReportID"] = agencyReport.ID;

            return RedirectToAction("CreateReportDetails");
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult CreateReportDetails()
        {
            int agencyID, agencyReportID;

            if (Session["agencyID"] != null && Session["agencyReportID"] != null)
            {
                agencyID = Convert.ToInt32(Session["agencyID"]);
                agencyReportID = Convert.ToInt32(Session["agencyReportID"]);

                ViewBag.agencyReportInfo = db.AgencyReport.Include(s => s.Agency)
                .SingleOrDefault(x => x.ID == agencyReportID);


                // show books agency has received
                var agencyBookDebt = db.AgencyBookDebts.Include(s => s.Book)
                                                        .Where(s => s.AgencyID == agencyID)
                                                        .ToList();
                List<Book> resultBooks = new List<Book>();

                foreach (var item in agencyBookDebt)
                {
                    resultBooks.Add(item.Book);
                }

                ViewBag.agencyBooks = new SelectList(resultBooks, "ID", "Name");

                List<AgencyReportDetail> agencyReportDetails = new List<AgencyReportDetail>();

                // if user has chosen a book from books list
                if (!string.IsNullOrWhiteSpace(Request.Form["bookID"]))
                {
                    int bookID;
                    Int32.TryParse(Request.Form["bookID"].ToString(), out bookID);

                    var agencyReportDetail = db.AgencyReportDetails
                                               .FirstOrDefault(s => s.AgencyReportID == agencyReportID
                                               && s.BookID == bookID);
                    AgencyBookDebt bookDebt = db.AgencyBookDebts
                                                        .Where(s => s.BookID == bookID && s.AgencyID == agencyID)
                                                        .FirstOrDefault();
                    int debtQuantity = bookDebt.Quantity;

                    if (agencyReportDetail != null)
                    {
                        // if chosen book already exists, increase its quantity by one
                        if ((agencyReportDetail.Quantity+1) <= debtQuantity)
                        {
                            agencyReportDetail.Quantity++;
                            db.SaveChanges();
                        }
                        else ViewBag.quantityError = "Số lượng báo cáo vượt quá số lượng còn nợ!" +
                                " Số nợ: "+debtQuantity+" cuốn";

                    }
                    else
                    {
                        if (debtQuantity != 0)
                        {
                            // if chosen book not exists, add new
                            Book book = db.Books.Find(bookID);
                            AgencyReportDetail a = new AgencyReportDetail
                            {
                                AgencyReportID = agencyReportID,
                                BookID = bookID,
                                Quantity = 1,
                                UnitPrice = book.SellingPrice
                            };
                            db.AgencyReportDetails.Add(a);
                            db.SaveChanges();
                        }
                        else ViewBag.quantityError = "Số lượng báo cáo vượt quá số lượng còn nợ." +
                                " Số nợ: "+0+" cuốn)";
                    }
                }
                // if user doesn't choose a book
                else
                {
                    if (Request.Form["justCreated"] != null && Request.Form["justCreated"] == "0")
                        // if this is not a newly created report with no details
                    {
                        agencyReportDetails = db.AgencyReportDetails
                        .Where(s => s.AgencyReportID == agencyReportID)
                        .ToList();
                        
                        for (int i = 0; i < agencyReportDetails.Count; i++)
                        {
                            int bookID = Convert.ToInt32(Request.Form["agencyReportDetail_" + i]);
                            int quantity = Convert.ToInt32(Request.Form["quantity_" + i]);

                            if(quantity > 0)
                            {
                                AgencyBookDebt bookDebt = db.AgencyBookDebts
                                                        .Where(s => s.BookID == bookID && s.AgencyID == agencyID)
                                                        .FirstOrDefault();
                                int debtQuantity = bookDebt.Quantity;

                                if (quantity <= debtQuantity)
                                {
                                    AgencyReportDetail a = agencyReportDetails.Where(s => s.BookID == bookID).FirstOrDefault();

                                    a.Quantity = quantity;

                                    db.SaveChanges();
                                }
                                else ViewBag.quantityError = "Số lượng báo cáo vượt quá số lượng còn nợ" +
                                " Số nợ: " + debtQuantity + " cuốn";
                            }
                           
                        }
                    }
                }

                agencyReportDetails = db.AgencyReportDetails
                        .Where(s => s.AgencyReportID == agencyReportID)
                        .Include(s => s.Book)
                        .ToList();

                int total = 0;
                foreach (var item in agencyReportDetails)
                {
                    total += item.Quantity * item.UnitPrice;
                }

                AgencyReport agencyReport = db.AgencyReport.Find(agencyReportID);
                agencyReport.Total = total;

                db.SaveChanges();

                return View(agencyReportDetails);
            }

            return RedirectToAction("Index");            
        }

        public ActionResult DeleteDetail(int bookID)
        {
            AgencyReportDetail a = db.AgencyReportDetails.Where(s => s.BookID == bookID).FirstOrDefault();
            db.AgencyReportDetails.Remove(a);
            db.SaveChanges();

            return RedirectToAction("CreateReportDetails");
        }

        public ActionResult DeleteReport(int reportID)
        {
            AgencyReport agencyReport = db.AgencyReport.Find(reportID);

            List<AgencyReportDetail> agencyReportDetails  = db.AgencyReportDetails
                            .Where(s => s.AgencyReportID == reportID).ToList();
            foreach(AgencyReportDetail i in agencyReportDetails)
            {
                db.AgencyReportDetails.Remove(i);
                db.SaveChanges();
            }

            db.AgencyReport.Remove(agencyReport);
            db.SaveChanges();

            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult Save(int reportID, int agencyID)
        {
            int total = 0;

            List<AgencyReportDetail> agencyReportDetails = db.AgencyReportDetails
                            .Where(s => s.AgencyReportID == reportID).ToList();
            // update angency book debt
            foreach (AgencyReportDetail i in agencyReportDetails)
            {
                AgencyBookDebt agencyBookDebt = db.AgencyBookDebts
                    .Where(s => s.BookID == i.BookID && s.AgencyID == agencyID)
                    .FirstOrDefault();
                agencyBookDebt.Quantity -= i.Quantity;

                db.SaveChanges();

                total += (i.Quantity * i.UnitPrice);
            }

            // update angency debt
            AgencyDebt agencyDebt = db.AgencyDebts
                            .Where(s => s.AgencyID == agencyID)
                            .OrderByDescending(s => s.Date)
                            .First();
            agencyDebt.Amount -= total; 
            agencyDebt.Date = DateTime.Now;

            db.AgencyDebts.Add(agencyDebt);
            db.SaveChanges();

            Session.Clear();

            return RedirectToAction("Index");
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
