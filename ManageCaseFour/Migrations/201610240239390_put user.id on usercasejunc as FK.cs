namespace ManageCaseFour.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class putuseridonusercasejuncasFK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserCaseJunctions", "id", c => c.String(maxLength: 128));
            CreateIndex("dbo.UserCaseJunctions", "id");
            AddForeignKey("dbo.UserCaseJunctions", "id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserCaseJunctions", "id", "dbo.AspNetUsers");
            DropIndex("dbo.UserCaseJunctions", new[] { "id" });
            DropColumn("dbo.UserCaseJunctions", "id");
        }
    }
}
