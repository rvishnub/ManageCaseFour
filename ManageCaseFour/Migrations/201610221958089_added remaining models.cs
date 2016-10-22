namespace ManageCaseFour.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedremainingmodels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cases",
                c => new
                    {
                        caseId = c.Int(nullable: false, identity: true),
                        caseName = c.String(),
                        caseNumber = c.String(),
                    })
                .PrimaryKey(t => t.caseId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        departmentId = c.Int(nullable: false, identity: true),
                        departmentCode = c.String(),
                        departmentName = c.String(),
                    })
                .PrimaryKey(t => t.departmentId);
            
            CreateTable(
                "dbo.DocumentSources",
                c => new
                    {
                        sourceId = c.Int(nullable: false, identity: true),
                        sourceCode = c.String(),
                        sourceName = c.String(),
                    })
                .PrimaryKey(t => t.sourceId);
            
            CreateTable(
                "dbo.DocumentTypes",
                c => new
                    {
                        typeId = c.Int(nullable: false, identity: true),
                        documentCode = c.String(),
                        documentName = c.String(),
                    })
                .PrimaryKey(t => t.typeId);
            
            CreateTable(
                "dbo.Facilities",
                c => new
                    {
                        facilityId = c.Int(nullable: false, identity: true),
                        facilityName = c.String(),
                    })
                .PrimaryKey(t => t.facilityId);
            
            CreateTable(
                "dbo.InternalCaseNumbers",
                c => new
                    {
                        internalCaseId = c.Int(nullable: false, identity: true),
                        internalCaseNumber = c.Int(nullable: false),
                        caseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.internalCaseId)
                .ForeignKey("dbo.Cases", t => t.caseId, cascadeDelete: true)
                .Index(t => t.caseId);
            
            CreateTable(
                "dbo.Principals",
                c => new
                    {
                        principalId = c.Int(nullable: false, identity: true),
                        principalCode = c.String(),
                        principalFirstName = c.String(),
                        principalLastName = c.String(),
                    })
                .PrimaryKey(t => t.principalId);
            
            CreateTable(
                "dbo.PrincipalCaseJunctions",
                c => new
                    {
                        principalCaseJunctionId = c.Int(nullable: false, identity: true),
                        principalId = c.Int(nullable: false),
                        caseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.principalCaseJunctionId)
                .ForeignKey("dbo.Cases", t => t.caseId, cascadeDelete: true)
                .ForeignKey("dbo.Principals", t => t.principalId, cascadeDelete: true)
                .Index(t => t.principalId)
                .Index(t => t.caseId);
            
            CreateTable(
                "dbo.UserCaseJunctions",
                c => new
                    {
                        userCaseJunctionId = c.Int(nullable: false, identity: true),
                        userId = c.Int(nullable: false),
                        caseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.userCaseJunctionId)
                .ForeignKey("dbo.Cases", t => t.caseId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.userId, cascadeDelete: true)
                .Index(t => t.userId)
                .Index(t => t.caseId);
            
            AddColumn("dbo.Records", "typeId", c => c.Int(nullable: false));
            AddColumn("dbo.Records", "providerFirstName", c => c.String());
            AddColumn("dbo.Records", "providerLastName", c => c.String());
            AddColumn("dbo.Records", "fileContent", c => c.String());
            AddColumn("dbo.Users", "userPIN", c => c.String());
            CreateIndex("dbo.Records", "internalCaseId");
            CreateIndex("dbo.Records", "sourceId");
            CreateIndex("dbo.Records", "departmentId");
            CreateIndex("dbo.Records", "typeId");
            CreateIndex("dbo.Records", "facilityId");
            AddForeignKey("dbo.Records", "departmentId", "dbo.Departments", "departmentId", cascadeDelete: true);
            AddForeignKey("dbo.Records", "sourceId", "dbo.DocumentSources", "sourceId", cascadeDelete: true);
            AddForeignKey("dbo.Records", "typeId", "dbo.DocumentTypes", "typeId", cascadeDelete: true);
            AddForeignKey("dbo.Records", "facilityId", "dbo.Facilities", "facilityId", cascadeDelete: true);
            AddForeignKey("dbo.Records", "internalCaseId", "dbo.InternalCaseNumbers", "internalCaseId", cascadeDelete: true);
            DropColumn("dbo.Records", "InternalCaseNumber");
            DropColumn("dbo.Records", "DocumentSource");
            DropColumn("dbo.Records", "Department");
            DropColumn("dbo.Records", "DocumentType");
            DropColumn("dbo.Records", "Facility");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Records", "Facility", c => c.String());
            AddColumn("dbo.Records", "DocumentType", c => c.String());
            AddColumn("dbo.Records", "Department", c => c.String());
            AddColumn("dbo.Records", "DocumentSource", c => c.String());
            AddColumn("dbo.Records", "InternalCaseNumber", c => c.String());
            DropForeignKey("dbo.UserCaseJunctions", "userId", "dbo.Users");
            DropForeignKey("dbo.UserCaseJunctions", "caseId", "dbo.Cases");
            DropForeignKey("dbo.Records", "internalCaseId", "dbo.InternalCaseNumbers");
            DropForeignKey("dbo.Records", "facilityId", "dbo.Facilities");
            DropForeignKey("dbo.Records", "typeId", "dbo.DocumentTypes");
            DropForeignKey("dbo.Records", "sourceId", "dbo.DocumentSources");
            DropForeignKey("dbo.Records", "departmentId", "dbo.Departments");
            DropForeignKey("dbo.PrincipalCaseJunctions", "principalId", "dbo.Principals");
            DropForeignKey("dbo.PrincipalCaseJunctions", "caseId", "dbo.Cases");
            DropForeignKey("dbo.InternalCaseNumbers", "caseId", "dbo.Cases");
            DropIndex("dbo.UserCaseJunctions", new[] { "caseId" });
            DropIndex("dbo.UserCaseJunctions", new[] { "userId" });
            DropIndex("dbo.Records", new[] { "facilityId" });
            DropIndex("dbo.Records", new[] { "typeId" });
            DropIndex("dbo.Records", new[] { "departmentId" });
            DropIndex("dbo.Records", new[] { "sourceId" });
            DropIndex("dbo.Records", new[] { "internalCaseId" });
            DropIndex("dbo.PrincipalCaseJunctions", new[] { "caseId" });
            DropIndex("dbo.PrincipalCaseJunctions", new[] { "principalId" });
            DropIndex("dbo.InternalCaseNumbers", new[] { "caseId" });
            DropColumn("dbo.Users", "userPIN");
            DropColumn("dbo.Records", "fileContent");
            DropColumn("dbo.Records", "providerLastName");
            DropColumn("dbo.Records", "providerFirstName");
            DropColumn("dbo.Records", "typeId");
            DropTable("dbo.UserCaseJunctions");
            DropTable("dbo.PrincipalCaseJunctions");
            DropTable("dbo.Principals");
            DropTable("dbo.InternalCaseNumbers");
            DropTable("dbo.Facilities");
            DropTable("dbo.DocumentTypes");
            DropTable("dbo.DocumentSources");
            DropTable("dbo.Departments");
            DropTable("dbo.Cases");
        }
    }
}
