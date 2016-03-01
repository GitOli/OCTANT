namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        IdAnswer = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                        Comment = c.String(),
                        Description = c.String(),
                        ConsultantComment = c.String(),
                        IdInterview = c.Int(nullable: false),
                        IdQuestion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdAnswer)
                .ForeignKey("dbo.Interviews", t => t.IdInterview, cascadeDelete: false)
                .ForeignKey("dbo.Questions", t => t.IdQuestion, cascadeDelete: false)
                .Index(t => t.IdInterview)
                .Index(t => t.IdQuestion);
            
            CreateTable(
                "dbo.Interviews",
                c => new
                    {
                        IdInterview = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Name = c.String(),
                        Comment = c.String(),
                        Canceled = c.Boolean(nullable: false),
                        Suspended = c.Boolean(nullable: false),
                        Description = c.String(),
                        Completion = c.Int(nullable: false),
                        IdCampaign = c.Int(nullable: false),
                        IdQuestionnaire = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdInterview)
                .ForeignKey("dbo.Campaigns", t => t.IdCampaign, cascadeDelete: false)
                .ForeignKey("dbo.Questionnaires", t => t.IdQuestionnaire, cascadeDelete: false)
                .Index(t => t.IdCampaign)
                .Index(t => t.IdQuestionnaire);
            
            CreateTable(
                "dbo.Campaigns",
                c => new
                    {
                        IdCampaign = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                        Comment = c.String(),
                        Coverage = c.Int(nullable: false),
                        IdManager = c.Int(nullable: false),
                        IdCustomer = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCampaign)
                .ForeignKey("dbo.Customers", t => t.IdCustomer, cascadeDelete: false)
                .Index(t => t.IdCustomer);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        IdCustomer = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Comment = c.String(),
                        IdIndustry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCustomer)
                .ForeignKey("dbo.Industries", t => t.IdIndustry, cascadeDelete: false)
                .Index(t => t.IdIndustry);
            
            CreateTable(
                "dbo.Industries",
                c => new
                    {
                        IdIndustry = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.IdIndustry);
            
            CreateTable(
                "dbo.Questionnaires",
                c => new
                    {
                        IdQuestionnaire = c.Int(nullable: false, identity: true),
                        IsDraft = c.Boolean(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Language = c.Int(nullable: false),
                        IdFramework = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdQuestionnaire)
                .ForeignKey("dbo.Frameworks", t => t.IdFramework, cascadeDelete: false)
                .Index(t => t.IdFramework);
            
            CreateTable(
                "dbo.Frameworks",
                c => new
                    {
                        IdFramework = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                        IdStandard = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdFramework)
                .ForeignKey("dbo.Standards", t => t.IdStandard, cascadeDelete: false)
                .Index(t => t.IdStandard);
            
            CreateTable(
                "dbo.Standards",
                c => new
                    {
                        IdStandard = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Version = c.String(),
                    })
                .PrimaryKey(t => t.IdStandard);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        IdQuestion = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        IsFactor = c.Boolean(nullable: false),
                        IsRelevant = c.Boolean(nullable: false),
                        AnswerType = c.Int(nullable: false),
                        IdRole = c.Int(nullable: false),
                        IdNode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdQuestion)
                .ForeignKey("dbo.Nodes", t => t.IdNode, cascadeDelete: false)
                .ForeignKey("dbo.Roles", t => t.IdRole, cascadeDelete: false)
                .Index(t => t.IdRole)
                .Index(t => t.IdNode);
            
            CreateTable(
                "dbo.Nodes",
                c => new
                    {
                        IdNode = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Weight = c.Int(nullable: false),
                        IsRoot = c.Boolean(nullable: false),
                        Equation = c.String(),
                        IdPerspective = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdNode)
                .ForeignKey("dbo.Perspectives", t => t.IdPerspective, cascadeDelete: false)
                .Index(t => t.IdPerspective);
            
            CreateTable(
                "dbo.Perspectives",
                c => new
                    {
                        IdPerspective = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Name = c.String(),
                        IsDefault = c.Boolean(nullable: false),
                        Weight = c.Int(nullable: false),
                        IdFramework = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPerspective)
                .ForeignKey("dbo.Frameworks", t => t.IdFramework, cascadeDelete: false)
                .Index(t => t.IdFramework);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        IdRole = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Name = c.String(),
                        IdFramework = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdRole)
                .ForeignKey("dbo.Frameworks", t => t.IdFramework, cascadeDelete: false)
                .Index(t => t.IdFramework);
            
            CreateTable(
                "dbo.Candidates",
                c => new
                    {
                        IdCandidate = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        PhoneNumber = c.Int(nullable: false),
                        EmailAddress = c.Int(nullable: false),
                        IdCustomer = c.Int(nullable: false),
                        IdGroup = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCandidate)
                .ForeignKey("dbo.Customers", t => t.IdCustomer, cascadeDelete: false)
                .ForeignKey("dbo.Groups", t => t.IdGroup, cascadeDelete: false)
                .Index(t => t.IdCustomer)
                .Index(t => t.IdGroup);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        IdGroup = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.IdGroup);
            
            CreateTable(
                "dbo.ConsultantCampaigns",
                c => new
                    {
                        IdUser = c.Int(nullable: false),
                        IdCampaign = c.Int(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => new { t.IdUser, t.IdCampaign })
                .ForeignKey("dbo.Campaigns", t => t.IdCampaign, cascadeDelete: false)
                .Index(t => t.IdCampaign);
            
            CreateTable(
                "dbo.Interviewees",
                c => new
                    {
                        IdCandidate = c.Int(nullable: false),
                        IdInterview = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdCandidate, t.IdInterview })
                .ForeignKey("dbo.Candidates", t => t.IdCandidate, cascadeDelete: false)
                .ForeignKey("dbo.Interviews", t => t.IdInterview, cascadeDelete: false)
                .Index(t => t.IdCandidate)
                .Index(t => t.IdInterview);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Interviewees", "IdInterview", "dbo.Interviews");
            DropForeignKey("dbo.Interviewees", "IdCandidate", "dbo.Candidates");
            DropForeignKey("dbo.ConsultantCampaigns", "IdCampaign", "dbo.Campaigns");
            DropForeignKey("dbo.Candidates", "IdGroup", "dbo.Groups");
            DropForeignKey("dbo.Candidates", "IdCustomer", "dbo.Customers");
            DropForeignKey("dbo.Answers", "IdQuestion", "dbo.Questions");
            DropForeignKey("dbo.Questions", "IdRole", "dbo.Roles");
            DropForeignKey("dbo.Roles", "IdFramework", "dbo.Frameworks");
            DropForeignKey("dbo.Questions", "IdNode", "dbo.Nodes");
            DropForeignKey("dbo.Nodes", "IdPerspective", "dbo.Perspectives");
            DropForeignKey("dbo.Perspectives", "IdFramework", "dbo.Frameworks");
            DropForeignKey("dbo.Answers", "IdInterview", "dbo.Interviews");
            DropForeignKey("dbo.Interviews", "IdQuestionnaire", "dbo.Questionnaires");
            DropForeignKey("dbo.Questionnaires", "IdFramework", "dbo.Frameworks");
            DropForeignKey("dbo.Frameworks", "IdStandard", "dbo.Standards");
            DropForeignKey("dbo.Interviews", "IdCampaign", "dbo.Campaigns");
            DropForeignKey("dbo.Campaigns", "IdCustomer", "dbo.Customers");
            DropForeignKey("dbo.Customers", "IdIndustry", "dbo.Industries");
            DropIndex("dbo.Interviewees", new[] { "IdInterview" });
            DropIndex("dbo.Interviewees", new[] { "IdCandidate" });
            DropIndex("dbo.ConsultantCampaigns", new[] { "IdCampaign" });
            DropIndex("dbo.Candidates", new[] { "IdGroup" });
            DropIndex("dbo.Candidates", new[] { "IdCustomer" });
            DropIndex("dbo.Roles", new[] { "IdFramework" });
            DropIndex("dbo.Perspectives", new[] { "IdFramework" });
            DropIndex("dbo.Nodes", new[] { "IdPerspective" });
            DropIndex("dbo.Questions", new[] { "IdNode" });
            DropIndex("dbo.Questions", new[] { "IdRole" });
            DropIndex("dbo.Frameworks", new[] { "IdStandard" });
            DropIndex("dbo.Questionnaires", new[] { "IdFramework" });
            DropIndex("dbo.Customers", new[] { "IdIndustry" });
            DropIndex("dbo.Campaigns", new[] { "IdCustomer" });
            DropIndex("dbo.Interviews", new[] { "IdQuestionnaire" });
            DropIndex("dbo.Interviews", new[] { "IdCampaign" });
            DropIndex("dbo.Answers", new[] { "IdQuestion" });
            DropIndex("dbo.Answers", new[] { "IdInterview" });
            DropTable("dbo.Interviewees");
            DropTable("dbo.ConsultantCampaigns");
            DropTable("dbo.Groups");
            DropTable("dbo.Candidates");
            DropTable("dbo.Roles");
            DropTable("dbo.Perspectives");
            DropTable("dbo.Nodes");
            DropTable("dbo.Questions");
            DropTable("dbo.Standards");
            DropTable("dbo.Frameworks");
            DropTable("dbo.Questionnaires");
            DropTable("dbo.Industries");
            DropTable("dbo.Customers");
            DropTable("dbo.Campaigns");
            DropTable("dbo.Interviews");
            DropTable("dbo.Answers");
        }
    }
}
