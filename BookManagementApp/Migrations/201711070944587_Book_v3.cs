namespace BookManagementApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Book_v3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Agencies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Address = c.String(),
                        Phone = c.String(),
                        AccountNumber = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AgencyReports",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AgencyID = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Agencies", t => t.AgencyID)
                .Index(t => t.AgencyID);
            
            CreateTable(
                "dbo.AgencyReportDetails",
                c => new
                    {
                        AgencyReportID = c.Int(nullable: false),
                        BookID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AgencyReportID, t.BookID })
                .ForeignKey("dbo.Books", t => t.BookID)
                .ForeignKey("dbo.AgencyReports", t => t.AgencyReportID)
                .Index(t => t.AgencyReportID)
                .Index(t => t.BookID);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PublisherID = c.Int(nullable: false),
                        GenreID = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Author = c.String(nullable: false),
                        PublicationDate = c.DateTime(nullable: false),
                        SellingPrice = c.Int(nullable: false),
                        PurchasePrice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Genres", t => t.GenreID)
                .ForeignKey("dbo.Publishers", t => t.PublisherID)
                .Index(t => t.PublisherID)
                .Index(t => t.GenreID);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Publishers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Address = c.String(),
                        Phone = c.String(),
                        AccountNumber = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Receipts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PublisherID = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Publishers", t => t.PublisherID)
                .Index(t => t.PublisherID);
            
            CreateTable(
                "dbo.ReceiptDetails",
                c => new
                    {
                        ReceiptID = c.Int(nullable: false),
                        BookID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ReceiptID, t.BookID })
                .ForeignKey("dbo.Books", t => t.BookID)
                .ForeignKey("dbo.Receipts", t => t.ReceiptID)
                .Index(t => t.ReceiptID)
                .Index(t => t.BookID);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PublisherID = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Publishers", t => t.PublisherID)
                .Index(t => t.PublisherID);
            
            CreateTable(
                "dbo.ReportDetails",
                c => new
                    {
                        ReportID = c.Int(nullable: false),
                        BookID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ReportID, t.BookID })
                .ForeignKey("dbo.Books", t => t.BookID)
                .ForeignKey("dbo.Reports", t => t.ReportID)
                .Index(t => t.ReportID)
                .Index(t => t.BookID);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AgencyID = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Agencies", t => t.AgencyID)
                .Index(t => t.AgencyID);
            
            CreateTable(
                "dbo.InvoiceDetails",
                c => new
                    {
                        InvoiceID = c.Int(nullable: false),
                        BookID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.InvoiceID, t.BookID })
                .ForeignKey("dbo.Books", t => t.BookID)
                .ForeignKey("dbo.Invoices", t => t.InvoiceID)
                .Index(t => t.InvoiceID)
                .Index(t => t.BookID);
            
            CreateTable(
                "dbo.AgencyBookDebts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AgencyID = c.Int(nullable: false),
                        BookID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Agencies", t => t.AgencyID)
                .ForeignKey("dbo.Books", t => t.BookID)
                .Index(t => t.AgencyID)
                .Index(t => t.BookID);
            
            CreateTable(
                "dbo.AgencyDebts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AgencyID = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Agencies", t => t.AgencyID)
                .Index(t => t.AgencyID);
            
            CreateTable(
                "dbo.Debts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PublisherID = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Publishers", t => t.PublisherID)
                .Index(t => t.PublisherID);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BookID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Books", t => t.BookID)
                .Index(t => t.BookID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stocks", "BookID", "dbo.Books");
            DropForeignKey("dbo.Debts", "PublisherID", "dbo.Publishers");
            DropForeignKey("dbo.AgencyDebts", "AgencyID", "dbo.Agencies");
            DropForeignKey("dbo.AgencyBookDebts", "BookID", "dbo.Books");
            DropForeignKey("dbo.AgencyBookDebts", "AgencyID", "dbo.Agencies");
            DropForeignKey("dbo.InvoiceDetails", "InvoiceID", "dbo.Invoices");
            DropForeignKey("dbo.InvoiceDetails", "BookID", "dbo.Books");
            DropForeignKey("dbo.Invoices", "AgencyID", "dbo.Agencies");
            DropForeignKey("dbo.AgencyReportDetails", "AgencyReportID", "dbo.AgencyReports");
            DropForeignKey("dbo.AgencyReportDetails", "BookID", "dbo.Books");
            DropForeignKey("dbo.ReportDetails", "ReportID", "dbo.Reports");
            DropForeignKey("dbo.ReportDetails", "BookID", "dbo.Books");
            DropForeignKey("dbo.Reports", "PublisherID", "dbo.Publishers");
            DropForeignKey("dbo.ReceiptDetails", "ReceiptID", "dbo.Receipts");
            DropForeignKey("dbo.ReceiptDetails", "BookID", "dbo.Books");
            DropForeignKey("dbo.Receipts", "PublisherID", "dbo.Publishers");
            DropForeignKey("dbo.Books", "PublisherID", "dbo.Publishers");
            DropForeignKey("dbo.Books", "GenreID", "dbo.Genres");
            DropForeignKey("dbo.AgencyReports", "AgencyID", "dbo.Agencies");
            DropIndex("dbo.Stocks", new[] { "BookID" });
            DropIndex("dbo.Debts", new[] { "PublisherID" });
            DropIndex("dbo.AgencyDebts", new[] { "AgencyID" });
            DropIndex("dbo.AgencyBookDebts", new[] { "BookID" });
            DropIndex("dbo.AgencyBookDebts", new[] { "AgencyID" });
            DropIndex("dbo.InvoiceDetails", new[] { "BookID" });
            DropIndex("dbo.InvoiceDetails", new[] { "InvoiceID" });
            DropIndex("dbo.Invoices", new[] { "AgencyID" });
            DropIndex("dbo.ReportDetails", new[] { "BookID" });
            DropIndex("dbo.ReportDetails", new[] { "ReportID" });
            DropIndex("dbo.Reports", new[] { "PublisherID" });
            DropIndex("dbo.ReceiptDetails", new[] { "BookID" });
            DropIndex("dbo.ReceiptDetails", new[] { "ReceiptID" });
            DropIndex("dbo.Receipts", new[] { "PublisherID" });
            DropIndex("dbo.Books", new[] { "GenreID" });
            DropIndex("dbo.Books", new[] { "PublisherID" });
            DropIndex("dbo.AgencyReportDetails", new[] { "BookID" });
            DropIndex("dbo.AgencyReportDetails", new[] { "AgencyReportID" });
            DropIndex("dbo.AgencyReports", new[] { "AgencyID" });
            DropTable("dbo.Stocks");
            DropTable("dbo.Debts");
            DropTable("dbo.AgencyDebts");
            DropTable("dbo.AgencyBookDebts");
            DropTable("dbo.InvoiceDetails");
            DropTable("dbo.Invoices");
            DropTable("dbo.ReportDetails");
            DropTable("dbo.Reports");
            DropTable("dbo.ReceiptDetails");
            DropTable("dbo.Receipts");
            DropTable("dbo.Publishers");
            DropTable("dbo.Genres");
            DropTable("dbo.Books");
            DropTable("dbo.AgencyReportDetails");
            DropTable("dbo.AgencyReports");
            DropTable("dbo.Agencies");
        }
    }
}
