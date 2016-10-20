namespace ManageCaseFour.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedtimestamp : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Audits", "Timestamp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Audits", "Timestamp", c => c.DateTime(nullable: false));
        }
    }
}
