namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Framework1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomScores",
                c => new
                    {
                        IdConsultant = c.Int(nullable: false),
                        IdNode = c.Int(nullable: false),
                        Value = c.Int(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => new { t.IdConsultant, t.IdNode })
                .ForeignKey("dbo.Nodes", t => t.IdNode, cascadeDelete: false)
                .Index(t => t.IdNode);
            
            CreateTable(
                "dbo.Propositions",
                c => new
                    {
                        IdProposition = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                        Description = c.String(),
                        IdQuestion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdProposition)
                .ForeignKey("dbo.Questions", t => t.IdQuestion, cascadeDelete: false)
                .Index(t => t.IdQuestion);
            
            AlterColumn("dbo.Interviews", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Industries", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Groups", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Firms", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Propositions", "IdQuestion", "dbo.Questions");
            DropForeignKey("dbo.CustomScores", "IdNode", "dbo.Nodes");
            DropIndex("dbo.Propositions", new[] { "IdQuestion" });
            DropIndex("dbo.CustomScores", new[] { "IdNode" });
            AlterColumn("dbo.Firms", "Name", c => c.String());
            AlterColumn("dbo.Groups", "Name", c => c.String());
            AlterColumn("dbo.Industries", "Name", c => c.String());
            AlterColumn("dbo.Customers", "Name", c => c.String());
            AlterColumn("dbo.Interviews", "Name", c => c.String());
            DropTable("dbo.Propositions");
            DropTable("dbo.CustomScores");
        }
    }
}
