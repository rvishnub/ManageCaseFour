namespace ManageCaseFour.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedusercasemanytomanyrelationforreal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationUserCases",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Case_caseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Case_caseId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Cases", t => t.Case_caseId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Case_caseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserCases", "Case_caseId", "dbo.Cases");
            DropForeignKey("dbo.ApplicationUserCases", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserCases", new[] { "Case_caseId" });
            DropIndex("dbo.ApplicationUserCases", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserCases");
        }
    }
}
