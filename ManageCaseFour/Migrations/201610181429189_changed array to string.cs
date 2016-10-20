namespace ManageCaseFour.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedarraytostring : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OCRs", "documentSections", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OCRs", "documentSections");
        }
    }
}
