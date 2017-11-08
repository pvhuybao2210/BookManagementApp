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
    public class ReceiptsController : Controller
    {
        private BookContext db = new BookContext();

        // GET: Receipts
        public ActionResult Index()
        {
            var receipts = db.Receipts.Include(r => r.Publisher);
            return View(receipts.ToList());
        }

        // GET: Receipts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receipt receipt = db.Receipts.Include(r => r.Publisher)
                                         .Include(r => r.ReceiptDetails.Select(q => q.Book))
                                         .SingleOrDefault(x => x.ID == id);
            if (receipt == null)
            {
                return HttpNotFound();
            }
            return View(receipt);
        }

        // GET: Receipts/Create
        public ActionResult Create()
        {
            ViewBag.publishers = new SelectList(db.Publishers, "ID", "Name");
            ViewBag.currentDate = DateTime.Now.ToString("dd/MM/yyyy h:mm:ss");

            return View();
        }

        [HttpPost]
        public ActionResult CreateReceipt()
        {
            Receipt receipt = new Receipt
            {
                PublisherID = Convert.ToInt32(Request.Form["publisherID"].ToString()),
                Total = 0,
                Date = DateTime.Now,
                Description = Request.Form["Description"].ToString(),
                Status = true
            };

            db.Receipts.Add(receipt);
            db.SaveChanges();

            Session["publisherID"] = receipt.PublisherID;
            Session["receiptID"] = receipt.ID;

            return RedirectToAction("CreateReceiptDetails");
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult CreateReceiptDetails()
        {
            int publisherID, receiptID;

            if (Session["publisherID"] != null && Session["receiptID"] != null)
            {
                publisherID = Convert.ToInt32(Session["publisherID"]);
                receiptID = Convert.ToInt32(Session["receiptID"]);

                ViewBag.receiptInfo = db.Receipts.Include(s => s.Publisher)
                .SingleOrDefault(x => x.ID == receiptID);


                // show books from chosen publisher
                var books = db.Books.Where(s => s.PublisherID == publisherID)
                                    .ToList();

                ViewBag.books = new SelectList(books, "ID", "Name");

                List<ReceiptDetail> receiptDetails = new List<ReceiptDetail>();

                // if user has chosen a book from books list
                if (!string.IsNullOrWhiteSpace(Request.Form["bookID"]))
                {
                    int bookID;
                    Int32.TryParse(Request.Form["bookID"].ToString(), out bookID);

                    var receiptDetail = db.ReceiptDetails
                                               .FirstOrDefault(s => s.ReceiptID == receiptID
                                               && s.BookID == bookID);

                    if (receiptDetail != null)
                    {
                        // if chosen book already exists, increase its quantity by one
                        receiptDetail.Quantity++;
                        db.SaveChanges();

                    }
                    else
                    {
                        // if chosen book not exists, add new
                        Book book = db.Books.Find(bookID);
                        ReceiptDetail a = new ReceiptDetail
                        {
                            ReceiptID = receiptID,
                            BookID = bookID,
                            Quantity = 1,
                            UnitPrice = book.PurchasePrice
                        };
                        db.ReceiptDetails.Add(a);
                        db.SaveChanges();
                    }
                }
                // if user doesn't choose a book
                else
                {
                    if (Request.Form["justCreated"] != null && Request.Form["justCreated"] == "0")
                    // if this is not a newly created report with no details
                    {
                        receiptDetails = db.ReceiptDetails
                        .Where(s => s.ReceiptID == receiptID)
                        .ToList();

                        for (int i = 0; i < receiptDetails.Count; i++)
                        {
                            int bookID = Convert.ToInt32(Request.Form["receiptDetail_" + i]);
                            int quantity = Convert.ToInt32(Request.Form["quantity_" + i]);

                            if (quantity > 0)
                            {
                                ReceiptDetail a = receiptDetails.Where(s => s.BookID == bookID).FirstOrDefault();

                                a.Quantity = quantity;

                                db.SaveChanges();
                            }

                        }
                    }
                }

                receiptDetails = db.ReceiptDetails
                        .Where(s => s.ReceiptID == receiptID)
                        .Include(s => s.Book)
                        .ToList();

                int total = 0;
                foreach (var item in receiptDetails)
                {
                    total += item.Quantity * item.UnitPrice;
                }

                Receipt receipt = db.Receipts.Find(receiptID);
                receipt.Total = total;

                db.SaveChanges();

                return View(receiptDetails);
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteDetail(int bookID)
        {
            ReceiptDetail a = db.ReceiptDetails.Where(s => s.BookID == bookID).FirstOrDefault();
            db.ReceiptDetails.Remove(a);
            db.SaveChanges();

            return RedirectToAction("CreateReceiptDetails");
        }

        public ActionResult DeleteReceipt(int receiptID)
        {
            Receipt receipt = db.Receipts.Find(receiptID);

            List<ReceiptDetail> receiptDetails = db.ReceiptDetails
                            .Where(s => s.ReceiptID == receiptID).ToList();
            foreach (ReceiptDetail i in receiptDetails)
            {
                db.ReceiptDetails.Remove(i);
                db.SaveChanges();
            }

            db.Receipts.Remove(receipt);
            db.SaveChanges();

            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult Save(int receiptID, int publisherID)
        {
            int total = 0;

            List<ReceiptDetail> receiptDetails = db.ReceiptDetails
                            .Where(s => s.ReceiptID == receiptID).ToList();
            // update stock
            foreach (ReceiptDetail i in receiptDetails)
            {
                Stock stock = db.Stocks
                    .Where(s => s.BookID == i.BookID)
                    .OrderByDescending(s => s.Date)
                    .FirstOrDefault();

                stock.Quantity += i.Quantity;
                stock.Date = DateTime.Now;

                db.Stocks.Add(stock);
                db.SaveChanges();

                total += (i.Quantity * i.UnitPrice);
            }

            // update debt
            Debt debt = db.Debts
                        .Where(s => s.PublisherID == publisherID)
                        .OrderByDescending(s => s.Date)
                        .First();
            debt.Amount += total;
            debt.Date = DateTime.Now;

            db.Debts.Add(debt);
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
