namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intervieweegrp : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Candidates", "IdGroup", "dbo.Groups");
            DropForeignKey("dbo.Interviewees", "IdGroup", "dbo.Groups");
            DropIndex("dbo.Candidates", new[] { "IdGroup" });
            DropIndex("dbo.Interviewees", new[] { "IdGroup" });
            AddColumn("dbo.CandidateCampaigns", "IdGroup", c => c.Int(nullable: true));
            CreateIndex("dbo.CandidateCampaigns", "IdGroup");
            AddForeignKey("dbo.CandidateCampaigns", "IdGroup", "dbo.Groups", "IdGroup", cascadeDelete: false);
            DropColumn("dbo.Candidates", "IdGroup");
            DropColumn("dbo.Interviewees", "IdGroup");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Interviewees", "IdGroup", c => c.Int());
            AddColumn("dbo.Candidates", "IdGroup", c => c.Int(nullable: false));
            DropForeignKey("dbo.CandidateCampaigns", "IdGroup", "dbo.Groups");
            DropIndex("dbo.CandidateCampaigns", new[] { "IdGroup" });
            DropColumn("dbo.CandidateCampaigns", "IdGroup");
            CreateIndex("dbo.Interviewees", "IdGroup");
            CreateIndex("dbo.Candidates", "IdGroup");
            AddForeignKey("dbo.Interviewees", "IdGroup", "dbo.Groups", "IdGroup");
            AddForeignKey("dbo.Candidates", "IdGroup", "dbo.Groups", "IdGroup", cascadeDelete: true);
        }
    }
}
