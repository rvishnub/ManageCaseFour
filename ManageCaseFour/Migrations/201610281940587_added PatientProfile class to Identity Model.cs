namespace ManageCaseFour.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedPatientProfileclasstoIdentityModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PatientProfiles",
                c => new
                    {
                        patientprofileId = c.Int(nullable: false, identity: true),
                        patientFirstName = c.String(),
                        patientLastName = c.String(),
                        dateOfBirth = c.DateTime(nullable: false),
                        dateOfAccident = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.patientprofileId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PatientProfiles");
        }
    }
}
