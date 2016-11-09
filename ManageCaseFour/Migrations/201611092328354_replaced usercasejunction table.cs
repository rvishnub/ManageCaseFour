namespace ManageCaseFour.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class replacedusercasejunctiontable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserCaseJunctions",
                c => new
                    {
                        userCaseJunctionId = c.Int(nullable: false, identity: true),
                        Id = c.String(maxLength: 128),
                        caseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.userCaseJunctionId)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Cases", t => t.caseId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.caseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserCaseJunctions", "caseId", "dbo.Cases");
            DropForeignKey("dbo.UserCaseJunctions", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserCaseJunctions", new[] { "caseId" });
            DropIndex("dbo.UserCaseJunctions", new[] { "Id" });
            DropTable("dbo.UserCaseJunctions");
        }
    }
}
