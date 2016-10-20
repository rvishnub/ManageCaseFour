namespace ManageCaseFour.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addednoteDatepropertytorecords : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Records", "noteDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Records", "noteDate");
        }
    }
}
