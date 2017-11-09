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
    public class StocksController : Controller
    {
        private BookContext db = new BookContext();

        public ActionResult Index()
        {
            var stocks = db.Stocks.ToList();
            var books = db.Books.ToList();
            List<Stock> stockResult = new List<Stock>();

            foreach (var b in books)
            {
                Stock stock = stocks.Where(s => s.BookID == b.ID).OrderByDescending(s => s.Date).First();
                stockResult.Add(stock);
            }

            ViewBag.Books = new SelectList(books, "ID", "Name");

            return View(stockResult.ToList());
        }

        public ActionResult StockFilter(FormCollection form)
        {
            var books = db.Books.ToList();
            ViewBag.Books = new SelectList(books, "ID", "Name");

            string tempBookID = Request.Form["Books"].ToString();
            int bookID = 0;
            if (tempBookID != "")
                bookID = Convert.ToInt32(tempBookID);

            DateTime date = DateTime.Now;
            if(!string.IsNullOrWhiteSpace(Request.Form["date"]))
            {
                var tempDate = Request.Form["FilterDate"].ToString();
                TimeSpan time = new TimeSpan(23, 59, 59);
                date = DateTime.Parse(tempDate).Add(time);
            }
           

            ViewBag.chosenDate = date;

            List<Stock> stockResult = new List<Stock>();

            if (bookID != 0)
            {
                Stock stock = db.Stocks.Include(s => s.Book)
                                        .Where(s => s.BookID == bookID
                                            && DbFunctions.TruncateTime(s.Date) <= date)
                                        .OrderByDescending(s => s.Date)
                                        .FirstOrDefault();
                stockResult.Add(stock);
                ViewBag.chosenBook = stock.Book.Name;
            }
            else
            {
                var stocks = db.Stocks.ToList();

                foreach (var b in books)
                {
                    Stock stock = stocks.Where(s => s.BookID == b.ID
                                            && (s.Date <= date))
                                        .OrderByDescending(s => s.Date).FirstOrDefault();
                    stockResult.Add(stock);
                }
                ViewBag.chosenBook = "Tất cả sách";
            }

            return View(stockResult);
        }
    }
}