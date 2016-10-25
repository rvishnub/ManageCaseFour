namespace ManageCaseFour.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeddocumentsource : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Records", "sourceId", "dbo.DocumentSources");
            DropIndex("dbo.Records", new[] { "sourceId" });
            DropColumn("dbo.Records", "sourceId");
            DropTable("dbo.DocumentSources");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DocumentSources",
                c => new
                    {
                        sourceId = c.Int(nullable: false, identity: true),
                        sourceCode = c.String(),
                        sourceName = c.String(),
                    })
                .PrimaryKey(t => t.sourceId);
            
            AddColumn("dbo.Records", "sourceId", c => c.Int(nullable: false));
            CreateIndex("dbo.Records", "sourceId");
            AddForeignKey("dbo.Records", "sourceId", "dbo.DocumentSources", "sourceId", cascadeDelete: true);
        }
    }
}
