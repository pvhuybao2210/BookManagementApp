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
    public class InvoicesController : Controller
    {
        private BookContext db = new BookContext();

        // GET: Invoices
        public ActionResult Index()
        {
            var invoices = db.Invoices.Include(i => i.Agency);
            return View(invoices.ToList());
        }

        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Include(r => r.Agency)
                                         .Include(r => r.InvoiceDetails.Select(q => q.Book))
                                         .SingleOrDefault(x => x.ID == id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            ViewBag.agencies = new SelectList(db.Agencies, "ID", "Name");
            ViewBag.currentDate = DateTime.Now.ToString("dd/MM/yyyy h:mm:ss");

            return View();
        }

        [HttpPost]
        public ActionResult CreateInvoice()
        {
            Invoice invoice = new Invoice
            {
                AgencyID = Convert.ToInt32(Request.Form["agencyID"].ToString()),
                Total = 0,
                Date = DateTime.Now,
                Description = Request.Form["Description"].ToString(),
                Status = true
            };

            db.Invoices.Add(invoice);
            db.SaveChanges();

            Session["agencyID"] = invoice.AgencyID;
            Session["invoiceID"] = invoice.ID;

            return RedirectToAction("CreateDetails");
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult CreateDetails()
        {
            int agencyID, invoiceID;

            if (Session["agencyID"] != null && Session["invoiceID"] != null)
            {
                agencyID = Convert.ToInt32(Session["agencyID"]);
                invoiceID = Convert.ToInt32(Session["invoiceID"]);

                ViewBag.invoiceInfo = db.Invoices.Include(s => s.Agency)
                .SingleOrDefault(x => x.ID == invoiceID);


                // show books from chosen publisher
                var books = db.Books.ToList();

                ViewBag.books = new SelectList(books, "ID", "Name");

                List<InvoiceDetail> invoiceDetails = new List<InvoiceDetail>();
                int total = 0;

                AgencyDebt agencyDebt = db.AgencyDebts.Find(agencyID);
                invoiceDetails = db.InvoiceDetails.Where(s => s.InvoiceID == invoiceID).ToList();
                foreach (var item in invoiceDetails)
                {
                    total += item.Quantity * item.UnitPrice;
                }

                ViewBag.agencyDebt = agencyDebt.Amount;

                // if user has chosen a book from books list
                if (!string.IsNullOrWhiteSpace(Request.Form["bookID"]))
                {
                    int bookID;
                    Int32.TryParse(Request.Form["bookID"].ToString(), out bookID);

                    var invoiceDetail = db.InvoiceDetails
                                               .FirstOrDefault(s => s.InvoiceID == invoiceID
                                               && s.BookID == bookID);

                    Book book = db.Books.Find(bookID);
                    Stock stock = db.Stocks.Where(s => s.BookID == bookID)
                                                        .OrderByDescending(s => s.Date)
                                                        .FirstOrDefault();
                    
                    if ( (invoiceDetail == null && stock.Quantity > 0) 
                        || (invoiceDetail != null && stock.Quantity >= invoiceDetail.Quantity) 
                       )
                    {
                        // check if agency debt is greater than total of creating invoice
                        if (agencyDebt.Amount > total)
                        {
                            if (invoiceDetail != null)
                            {
                                // if chosen book already exists, increase its quantity by one
                                invoiceDetail.Quantity++;
                                db.SaveChanges();

                            }
                            else
                            {
                                // if chosen book not exists, add new
                                InvoiceDetail a = new InvoiceDetail
                                {
                                    InvoiceID = invoiceID,
                                    BookID = bookID,
                                    Quantity = 1,
                                    UnitPrice = book.SellingPrice
                                };
                                db.InvoiceDetails.Add(a);

                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            ViewBag.error = "Tổng tiền vượt quá số tiền còn nợ! ";
                            ViewBag.errorInfo = agencyDebt.Amount.ToString();
                        }
                    }
                    else
                    {
                        ViewBag.error = "Không đủ sách! ";
                        ViewBag.errorInfo = stock.Quantity.ToString();
                    }
                }
                // if user doesn't choose a book
                else
                {
                    if (Request.Form["justCreated"] != null && Request.Form["justCreated"] == "0")
                    // if this is not a newly created report with no details
                    {
                        invoiceDetails = db.InvoiceDetails
                        .Where(s => s.InvoiceID == invoiceID)
                        .ToList();

                        for (int i = 0; i < invoiceDetails.Count; i++)
                        {
                            int bookID = Convert.ToInt32(Request.Form["invoiceDetail_" + i]);
                            int quantity = Convert.ToInt32(Request.Form["quantity_" + i]);

                            Book book = db.Books.Find(bookID);
                            
                            if (quantity > 0)
                            {
                                Stock stock = db.Stocks.Where(s => s.BookID == bookID)
                                                        .OrderByDescending(s => s.Date).FirstOrDefault();
                                if (stock.Quantity >= quantity)
                                {
                                    if (agencyDebt.Amount > (total + quantity * book.SellingPrice))
                                    {
                                        InvoiceDetail a = invoiceDetails.Where(s => s.BookID == bookID).FirstOrDefault();

                                        a.Quantity = quantity;

                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        ViewBag.error = "Tổng tiền vượt quá số tiền còn nợ! ";
                                        ViewBag.errorInfo = agencyDebt.Amount.ToString();
                                    }
                                }
                                else
                                {
                                    ViewBag.error = "Không đủ sách ";
                                    ViewBag.errorInfo = stock.Quantity.ToString();
                                   
                                }
                            }

                        }
                    }
                }

                invoiceDetails = db.InvoiceDetails
                        .Where(s => s.InvoiceID == invoiceID)
                        .Include(s => s.Book)
                        .ToList();

                // re-count total
                total = 0;

                foreach (var item in invoiceDetails)
                {
                    total += item.Quantity * item.UnitPrice;
                }

                Invoice invoice = db.Invoices.Find(invoiceID);
                invoice.Total = total;

                db.SaveChanges();

                return View(invoiceDetails);
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteDetail(int bookID)
        {
            InvoiceDetail a = db.InvoiceDetails.Where(s => s.BookID == bookID).FirstOrDefault();
            db.InvoiceDetails.Remove(a);
            db.SaveChanges();

            return RedirectToAction("CreateDetails");
        }

        public ActionResult DeleteInvoice(int invoiceID)
        {
            Invoice invoice = db.Invoices.Find(invoiceID);

            List<InvoiceDetail> invoiceDetails = db.InvoiceDetails
                            .Where(s => s.InvoiceID == invoiceID).ToList();
            foreach (InvoiceDetail i in invoiceDetails)
            {
                db.InvoiceDetails.Remove(i);
                db.SaveChanges();
            }

            db.Invoices.Remove(invoice);
            db.SaveChanges();

            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult Save(int invoiceID, int agencyID)
        {
            int total = 0;

            List<InvoiceDetail> invoiceDetails = db.InvoiceDetails
                            .Where(s => s.InvoiceID == invoiceID).ToList();
            // update stock
            foreach (InvoiceDetail i in invoiceDetails)
            {
                Stock stock = db.Stocks
                    .Where(s => s.BookID == i.BookID)
                    .OrderByDescending(s => s.Date)
                    .FirstOrDefault();

                stock.Quantity -= i.Quantity;

                db.Stocks.Add(stock);
                db.SaveChanges();

                total += (i.Quantity * i.UnitPrice);
            }

            // update debt
            AgencyDebt debt = db.AgencyDebts
                        .Where(s => s.AgencyID == agencyID)
                        .OrderByDescending(s => s.Date)
                        .First();
            debt.Amount += total;
            debt.Date = DateTime.Now;

            db.AgencyDebts.Add(debt);
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
