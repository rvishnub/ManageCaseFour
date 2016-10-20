namespace ManageCaseFour.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedrecordclass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Records",
                c => new
                    {
                        recordId = c.Int(nullable: false, identity: true),
                        internalCaseId = c.Int(nullable: false),
                        InternalCaseNumber = c.String(),
                        sourceId = c.Int(nullable: false),
                        DocumentSource = c.String(),
                        departmentId = c.Int(nullable: false),
                        Department = c.String(),
                        documentId = c.String(),
                        DocumentType = c.String(),
                        facilityId = c.Int(nullable: false),
                        Facility = c.String(),
                        recordReferenceNumber = c.String(),
                        pageNumber = c.String(),
                        recordEntryDate = c.DateTime(nullable: false),
                        provider = c.String(),
                        memo = c.String(),
                        serviceDate = c.DateTime(nullable: false),
                        noteSubjective = c.String(),
                        history = c.String(),
                        noteObjective = c.String(),
                        noteAssessment = c.String(),
                        notePlan = c.String(),
                        medications = c.String(),
                        age = c.String(),
                        DOB = c.String(),
                        allergies = c.String(),
                        vitalSigns = c.String(),
                        diagnosis = c.String(),
                        fileContent = c.String(),
                    })
                .PrimaryKey(t => t.recordId);
            
            AlterColumn("dbo.OCRs", "documentId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OCRs", "documentId", c => c.Int(nullable: false));
            DropTable("dbo.Records");
        }
    }
}
