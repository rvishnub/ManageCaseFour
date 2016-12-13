namespace ManageCaseFour.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cryptoclass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cryptoes",
                c => new
                    {
                        arrayId = c.Int(nullable: false, identity: true),
                        filename = c.String(),
                        encryptedOriginal = c.Binary(),
                        key = c.Binary(),
                        IV = c.Binary(),
                    })
                .PrimaryKey(t => t.arrayId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Cryptoes");
        }
    }
}
