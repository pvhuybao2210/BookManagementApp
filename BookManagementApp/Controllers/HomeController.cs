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

        // GET: Stocks
        public ActionResult Index()
        {
            var stocks = db.Stocks.ToList(); 
            var books = db.Books.ToList();
            List<Stock> stockResult = new List<Stock>();

            foreach (var b in books)
            {
                Stock stock = stocks.Where(s => s.BookID == b.ID).OrderBy(s => s.Date).Last();
                stockResult.Add(stock);
            }

            //var stocks = db.Stocks.Include(s => s.Book);

            return View(stockResult.ToList());
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