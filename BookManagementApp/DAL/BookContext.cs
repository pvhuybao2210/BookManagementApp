using BookManagementApp.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BookManagementApp.DAL
{
    public class BookContext : DbContext
    {

        public BookContext() : base("BookManagement")
        {
            //Database.SetInitializer(new BookInitializer());
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptDetail> ReceiptDetails { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportDetail> ReportDetails { get; set; }

        public DbSet<Agency> Agencies { get; set; }    
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<AgencyReport> AgencyReport { get; set; }
        public DbSet<AgencyReportDetail> AgencyReportDetails { get; set; }
        public DbSet<AgencyDebt> AgencyDebts { get; set; }
        public DbSet<AgencyBookDebt> AgencyBookDebts { get; set; }

        public DbSet<Debt> Debts { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToOneConstraintIntroductionConvention>();
        }

    }
}