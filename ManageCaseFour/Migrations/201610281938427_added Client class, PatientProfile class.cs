namespace ManageCaseFour.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedClientclassPatientProfileclass : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Records", "typeId", "dbo.DocumentTypes");
            DropIndex("dbo.Records", new[] { "typeId" });
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        clientId = c.Int(nullable: false, identity: true),
                        clientName = c.String(),
                        principalId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.clientId)
                .ForeignKey("dbo.Principals", t => t.principalId, cascadeDelete: true)
                .Index(t => t.principalId);
            
            DropColumn("dbo.Records", "typeId");
            DropTable("dbo.DocumentTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DocumentTypes",
                c => new
                    {
                        typeId = c.Int(nullable: false, identity: true),
                        documentCode = c.String(),
                        documentName = c.String(),
                    })
                .PrimaryKey(t => t.typeId);
            
            AddColumn("dbo.Records", "typeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Clients", "principalId", "dbo.Principals");
            DropIndex("dbo.Clients", new[] { "principalId" });
            DropTable("dbo.Clients");
            CreateIndex("dbo.Records", "typeId");
            AddForeignKey("dbo.Records", "typeId", "dbo.DocumentTypes", "typeId", cascadeDelete: true);
        }
    }
}
