namespace ManageCaseFour.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedidtoId : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.UserCaseJunctions", new[] { "id" });
            CreateIndex("dbo.UserCaseJunctions", "Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserCaseJunctions", new[] { "Id" });
            CreateIndex("dbo.UserCaseJunctions", "id");
        }
    }
}
