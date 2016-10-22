namespace ManageCaseFour.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedusermodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        userId = c.Int(nullable: false, identity: true),
                        userName = c.Int(nullable: false),
                        userLastName = c.String(),
                        userFirstName = c.String(),
                        userLogin = c.String(),
                        userPassword = c.String(),
                        userEmail = c.String(),
                        userPosition = c.String(),
                        userSecurityQuestion = c.String(),
                        userSecurityAnswer = c.String(),
                    })
                .PrimaryKey(t => t.userId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
