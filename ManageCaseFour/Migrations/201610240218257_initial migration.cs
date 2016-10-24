namespace ManageCaseFour.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialmigration : DbMigration
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
                        county = c.String(),
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
                        caseEntryDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.internalCaseId)
                .ForeignKey("dbo.Cases", t => t.caseId, cascadeDelete: true)
                .Index(t => t.caseId);
            
            CreateTable(
                "dbo.OCRs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        documentId = c.String(),
                        documentFilename = c.String(),
                        documentText = c.String(),
                        documentSections = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
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
                "dbo.Records",
                c => new
                    {
                        recordId = c.Int(nullable: false, identity: true),
                        internalCaseId = c.Int(nullable: false),
                        sourceId = c.Int(nullable: false),
                        departmentId = c.Int(nullable: false),
                        typeId = c.Int(nullable: false),
                        facilityId = c.Int(nullable: false),
                        documentId = c.String(),
                        recordReferenceNumber = c.String(),
                        pageNumber = c.String(),
                        recordEntryDate = c.DateTime(nullable: false),
                        provider = c.String(),
                        memo = c.String(),
                        noteDate = c.String(),
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
                        providerFirstName = c.String(),
                        providerLastName = c.String(),
                        fileContent = c.String(),
                        diagnosis = c.String(),
                    })
                .PrimaryKey(t => t.recordId)
                .ForeignKey("dbo.Departments", t => t.departmentId, cascadeDelete: true)
                .ForeignKey("dbo.DocumentSources", t => t.sourceId, cascadeDelete: true)
                .ForeignKey("dbo.DocumentTypes", t => t.typeId, cascadeDelete: true)
                .ForeignKey("dbo.Facilities", t => t.facilityId, cascadeDelete: true)
                .ForeignKey("dbo.InternalCaseNumbers", t => t.internalCaseId, cascadeDelete: true)
                .Index(t => t.internalCaseId)
                .Index(t => t.sourceId)
                .Index(t => t.departmentId)
                .Index(t => t.typeId)
                .Index(t => t.facilityId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserCaseJunctions",
                c => new
                    {
                        userCaseJunctionId = c.Int(nullable: false, identity: true),
                        caseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.userCaseJunctionId)
                .ForeignKey("dbo.Cases", t => t.caseId, cascadeDelete: true)
                .Index(t => t.caseId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCaseJunctions", "caseId", "dbo.Cases");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Records", "internalCaseId", "dbo.InternalCaseNumbers");
            DropForeignKey("dbo.Records", "facilityId", "dbo.Facilities");
            DropForeignKey("dbo.Records", "typeId", "dbo.DocumentTypes");
            DropForeignKey("dbo.Records", "sourceId", "dbo.DocumentSources");
            DropForeignKey("dbo.Records", "departmentId", "dbo.Departments");
            DropForeignKey("dbo.PrincipalCaseJunctions", "principalId", "dbo.Principals");
            DropForeignKey("dbo.PrincipalCaseJunctions", "caseId", "dbo.Cases");
            DropForeignKey("dbo.InternalCaseNumbers", "caseId", "dbo.Cases");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.UserCaseJunctions", new[] { "caseId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Records", new[] { "facilityId" });
            DropIndex("dbo.Records", new[] { "typeId" });
            DropIndex("dbo.Records", new[] { "departmentId" });
            DropIndex("dbo.Records", new[] { "sourceId" });
            DropIndex("dbo.Records", new[] { "internalCaseId" });
            DropIndex("dbo.PrincipalCaseJunctions", new[] { "caseId" });
            DropIndex("dbo.PrincipalCaseJunctions", new[] { "principalId" });
            DropIndex("dbo.InternalCaseNumbers", new[] { "caseId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UserCaseJunctions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Records");
            DropTable("dbo.PrincipalCaseJunctions");
            DropTable("dbo.Principals");
            DropTable("dbo.OCRs");
            DropTable("dbo.InternalCaseNumbers");
            DropTable("dbo.Facilities");
            DropTable("dbo.DocumentTypes");
            DropTable("dbo.DocumentSources");
            DropTable("dbo.Departments");
            DropTable("dbo.Cases");
        }
    }
}
