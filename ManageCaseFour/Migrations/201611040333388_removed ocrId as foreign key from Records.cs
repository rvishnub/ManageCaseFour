namespace ManageCaseFour.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedocrIdasforeignkeyfromRecords : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Records", "ocrId", "dbo.OCRs");
            DropIndex("dbo.Records", new[] { "ocrId" });
            DropColumn("dbo.Records", "ocrId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Records", "ocrId", c => c.Int(nullable: false));
            CreateIndex("dbo.Records", "ocrId");
            AddForeignKey("dbo.Records", "ocrId", "dbo.OCRs", "ocrId", cascadeDelete: true);
        }
    }
}
