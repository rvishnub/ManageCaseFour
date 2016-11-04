namespace ManageCaseFour.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedrecordIdasforeignkeytoOCR : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OCRs", "recordId", c => c.Int(nullable: false));
            CreateIndex("dbo.OCRs", "recordId");
            AddForeignKey("dbo.OCRs", "recordId", "dbo.Records", "recordId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OCRs", "recordId", "dbo.Records");
            DropIndex("dbo.OCRs", new[] { "recordId" });
            DropColumn("dbo.OCRs", "recordId");
        }
    }
}
