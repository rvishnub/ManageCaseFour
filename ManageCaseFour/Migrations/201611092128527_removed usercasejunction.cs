namespace ManageCaseFour.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedusercasejunction : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserCaseJunctions", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCaseJunctions", "caseId", "dbo.Cases");
            DropIndex("dbo.UserCaseJunctions", new[] { "Id" });
            DropIndex("dbo.UserCaseJunctions", new[] { "caseId" });
            DropTable("dbo.UserCaseJunctions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserCaseJunctions",
                c => new
                    {
                        userCaseJunctionId = c.Int(nullable: false, identity: true),
                        Id = c.String(maxLength: 128),
                        caseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.userCaseJunctionId);
            
            CreateIndex("dbo.UserCaseJunctions", "caseId");
            CreateIndex("dbo.UserCaseJunctions", "Id");
            AddForeignKey("dbo.UserCaseJunctions", "caseId", "dbo.Cases", "caseId", cascadeDelete: true);
            AddForeignKey("dbo.UserCaseJunctions", "Id", "dbo.AspNetUsers", "Id");
        }
    }
}
