using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BookManagementApp.Models;

namespace BookManagementApp.DAL
{
    public class BookInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<BookContext>
    {
        protected override void Seed(BookContext context)
        {
            // Genre
            var genres = new List<Genre>
            {        
                new Genre{Name="Khoa học"},
                new Genre{Name="Kinh dị"},
                new Genre{Name="Kỹ năng mềm"},
                new Genre{Name="Truyện tranh"},
            };

            genres.ForEach(s => context.Genres.Add(s));
            context.SaveChanges();

            // Publiser
            var publishers = new List<Publisher>
            {
                new Publisher{Name = "Kim Đồng", Address = "abc", Phone = "123", AccountNumber = "02312"},
                new Publisher{Name = "Trẻ", Address = "xyz", Phone = "456", AccountNumber = "1321"},
                new Publisher{Name = "Tổng hợp", Address = "xyz", Phone = "456", AccountNumber = "1321"},
                new Publisher{Name = "Đại học Quốc gia", Address = "xyz", Phone = "456", AccountNumber = "1321"},
            };

            publishers.ForEach(s => context.Publishers.Add(s));
            context.SaveChanges();

            // Agency
            var agencies = new List<Agency>
            {
                new Agency{Name = "Đại lý 1", Address = "uerowi", Phone = "2343", AccountNumber = "01232312"},
                new Agency{Name = "Đại lý 2", Address = "puqp", Phone = "4532526", AccountNumber = "1432321"},
                new Agency{Name = "Đại lý 3", Address = "fads", Phone = "4532526", AccountNumber = "1432321"},
                new Agency{Name = "Đại lý 4", Address = "puqwerwep", Phone = "4532526", AccountNumber = "1432321"},
            };

            agencies.ForEach(s => context.Agencies.Add(s));
            context.SaveChanges();
            /*
             Genres = new List<Genre>(){
                         genres.Single(u => u.ID == 3),
                         genres.Single(u => u.ID == 5)
                    }
             */

            // Book
            var books = new List<Book>
             {
                 new Book{PublisherID = 1, GenreID = 3, Name = "Đắc nhân tâm", Author = "Dale Carnegie",
                     PublicationDate = DateTime.Parse("2008-09-01"),
                     SellingPrice = 100000, PurchasePrice = 80000
                 },
                 new Book{PublisherID = 1, GenreID = 3, Name = "Quẳng gánh lo đi mà vui sống", Author = "Dale Carnegie",
                     PublicationDate = DateTime.Parse("2010-7-8"),
                     SellingPrice = 89000, PurchasePrice = 70000
                 },
                 new Book{PublisherID = 2, GenreID = 4, Name = "Doraemon", Author = "Fujiko Fujio",
                     PublicationDate = DateTime.Parse("2001-11-24"),
                     SellingPrice = 23000, PurchasePrice = 15000
                 },            
                 new Book{PublisherID = 2, GenreID = 4, Name = "Thám tử lừng danh Conan", Author = "Aoyama Gosho",
                     PublicationDate = DateTime.Parse("2002-2-10"),
                     SellingPrice = 28000, PurchasePrice = 18000 
                 },
                 new Book{PublisherID = 3, GenreID = 1, Name = "Lược sử thời gian", Author = "Stephen Hawking",
                     PublicationDate = DateTime.Parse("2008-2-13"),
                     SellingPrice = 70000, PurchasePrice = 65000
                 },
                 new Book{PublisherID = 3, GenreID = 1, Name = "Nguồn gốc các loài", Author = "Charles Darwin",
                     PublicationDate = DateTime.Parse("2011-7-11"),
                     SellingPrice = 83000, PurchasePrice = 72500
                 },
                 new Book{PublisherID = 4, GenreID = 2, Name = "IT", Author = "Stephen King",
                     PublicationDate = DateTime.Parse("2012-12-31"),
                     SellingPrice = 105000, PurchasePrice = 90000
                 },
                 new Book{PublisherID = 4, GenreID = 2, Name = "Dracula", Author = "Bram Stocker",
                     PublicationDate = DateTime.Parse("2014-2-13"),
                     SellingPrice = 125000, PurchasePrice = 100000
                 }
             };

             books.ForEach(s => context.Books.Add(s));
             context.SaveChanges();

            // Receipt
            var receipts = new List<Receipt>
             {
                 new Receipt{PublisherID = 1, Total = 15000000, Date = DateTime.Parse("2017-10-28 10:53"), Description = ""
                 , Status = true},
                 new Receipt{PublisherID = 2, Total = 3300000, Date = DateTime.Parse("2017-10-29 09:24"), Description = ""
                 , Status = true},
                 new Receipt{PublisherID = 3, Total = 13750000, Date = DateTime.Parse("2017-10-29 11:36"), Description = ""
                 , Status = true},
                 new Receipt{PublisherID = 4, Total = 19000000, Date = DateTime.Parse("2017-10-30 08:12"), Description = ""
                 , Status = true},
             }; 

            receipts.ForEach(s => context.Receipts.Add(s));
            context.SaveChanges();

            // Receipt detail
            var receiptDetails = new List<ReceiptDetail>
             {
                 new ReceiptDetail{ReceiptID = 1, BookID = 1, Quantity = 100, UnitPrice = 80000},
                 new ReceiptDetail{ReceiptID = 1, BookID = 2, Quantity = 100, UnitPrice = 70000},
                 new ReceiptDetail{ReceiptID = 2, BookID = 3, Quantity = 100, UnitPrice = 15000},
                 new ReceiptDetail{ReceiptID = 2, BookID = 4, Quantity = 100, UnitPrice = 18000},
                 new ReceiptDetail{ReceiptID = 3, BookID = 5, Quantity = 100, UnitPrice = 65000},
                 new ReceiptDetail{ReceiptID = 3, BookID = 6, Quantity = 100, UnitPrice = 72500},
                 new ReceiptDetail{ReceiptID = 4, BookID = 7, Quantity = 100, UnitPrice = 90000},
                 new ReceiptDetail{ReceiptID = 4, BookID = 8, Quantity = 100, UnitPrice = 100000},
             };

            receiptDetails.ForEach(s => context.ReceiptDetails.Add(s));
            context.SaveChanges();

            // Invoice
            var invoices = new List<Invoice>
             {
                 new Invoice{AgencyID = 1, Total = 2335000, Date = DateTime.Parse("2017-11-1 07:55"), Description = ""
                 , Status = true},
                 new Invoice{AgencyID = 2, Total = 3865000, Date = DateTime.Parse("2017-11-2 14:33"), Description = ""
                 , Status = true}
             };

            invoices.ForEach(s => context.Invoices.Add(s));
            context.SaveChanges();

            // Invoice detail
            var invoiceDetails = new List<InvoiceDetail>
             {
                 new InvoiceDetail{InvoiceID = 1, BookID = 1, Quantity = 10, UnitPrice = 100000},
                 new InvoiceDetail{InvoiceID = 1, BookID = 2, Quantity = 15, UnitPrice = 89000},
                 new InvoiceDetail{InvoiceID = 2, BookID = 3, Quantity = 25, UnitPrice = 23000},
                 new InvoiceDetail{InvoiceID = 2, BookID = 4, Quantity = 30, UnitPrice = 28000},
                 new InvoiceDetail{InvoiceID = 2, BookID = 5, Quantity = 35, UnitPrice = 70000},
             };

            invoiceDetails.ForEach(s => context.InvoiceDetails.Add(s));
            context.SaveChanges();

            // Stock
            var stock = new List<Stock>
             {
                // nhap
                 new Stock{BookID = 1, Quantity = 100, Date = DateTime.Parse("2017-10-28 10:53")},
                 new Stock{BookID = 2, Quantity = 100, Date = DateTime.Parse("2017-10-28 10:53")},
                 new Stock{BookID = 3, Quantity = 100, Date = DateTime.Parse("2017-10-29 09:24")},
                 new Stock{BookID = 4, Quantity = 100, Date = DateTime.Parse("2017-10-29 09:24")},
                 new Stock{BookID = 5, Quantity = 100, Date = DateTime.Parse("2017-10-29 11:36")},
                 new Stock{BookID = 6, Quantity = 100, Date = DateTime.Parse("2017-10-29 11:36")},
                 new Stock{BookID = 7, Quantity = 100, Date = DateTime.Parse("2017-10-30 08:12")},
                 new Stock{BookID = 8, Quantity = 100, Date = DateTime.Parse("2017-10-30 08:12")},
                 // xuat
                 new Stock{BookID = 1, Quantity = 90, Date = DateTime.Parse("2017-11-1 07:55")},
                 new Stock{BookID = 2, Quantity = 85, Date = DateTime.Parse("2017-11-1 07:55")},
                 new Stock{BookID = 3, Quantity = 75, Date = DateTime.Parse("2017-11-2 14:33")},
                 new Stock{BookID = 4, Quantity = 70, Date = DateTime.Parse("2017-11-2 14:33")},
                 new Stock{BookID = 5, Quantity = 65, Date = DateTime.Parse("2017-11-2 14:33")},
             };

            stock.ForEach(s => context.Stocks.Add(s));
            context.SaveChanges();

            // Agency book debt
            var agenyBookDebt = new List<AgencyBookDebt>
            {
                 new AgencyBookDebt{AgencyID = 1, BookID = 1, Quantity = 10 },
                 new AgencyBookDebt{AgencyID = 1, BookID = 2, Quantity = 15 },
                 new AgencyBookDebt{AgencyID = 2, BookID = 3, Quantity = 25 },
                 new AgencyBookDebt{AgencyID = 2, BookID = 4, Quantity = 30 },
                 new AgencyBookDebt{AgencyID = 2, BookID = 5, Quantity = 35 },
            };

            agenyBookDebt.ForEach(s => context.AgencyBookDebts.Add(s));
            context.SaveChanges();

            // Agency debt
            var agenyDebt = new List<AgencyDebt>
            {
                 new AgencyDebt{AgencyID = 1, Amount = 2335000, Date = DateTime.Parse("2017-11-1 07:55") },
                 new AgencyDebt{AgencyID = 2, Amount = 3865000, Date = DateTime.Parse("2017-11-2 14:33") }, 
            };

            agenyDebt.ForEach(s => context.AgencyDebts.Add(s));
            context.SaveChanges();

            // Agency debt
            var debt = new List<Debt>
            {
                 new Debt{PublisherID = 1, Amount = 15000000, Date = DateTime.Parse("2017-10-28 10:53") },
                 new Debt{PublisherID = 2, Amount = 3300000, Date = DateTime.Parse("2017-10-29 09:24") },
                 new Debt{PublisherID = 3, Amount = 13750000, Date = DateTime.Parse("2017-10-29 11:36") },
                 new Debt{PublisherID = 4, Amount = 19000000, Date = DateTime.Parse("2017-10-30 08:12") },
            };

            debt.ForEach(s => context.Debts.Add(s));
            context.SaveChanges();
        }
    }
}