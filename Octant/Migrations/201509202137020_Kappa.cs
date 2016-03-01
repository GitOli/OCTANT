namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Kappa : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Candidates", "IdCandidate", "dbo.Campaigns");
            DropForeignKey("dbo.CandidateCampaigns", "IdCandidate", "dbo.Candidates");
            DropForeignKey("dbo.Interviewees", "IdCandidate", "dbo.Candidates");
            DropIndex("dbo.Candidates", new[] { "IdCandidate" });
            DropPrimaryKey("dbo.Candidates");
            //AddColumn("dbo.Candidates", "Campaign_IdCampaign", c => c.Int());
            AlterColumn("dbo.Candidates", "IdCandidate", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Candidates", "IdCandidate");
            //CreateIndex("dbo.Candidates", "Campaign_IdCampaign");
            //AddForeignKey("dbo.Candidates", "Campaign_IdCampaign", "dbo.Campaigns", "IdCampaign");
            AddForeignKey("dbo.CandidateCampaigns", "IdCandidate", "dbo.Candidates", "IdCandidate", cascadeDelete: true);
            AddForeignKey("dbo.Interviewees", "IdCandidate", "dbo.Candidates", "IdCandidate", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Interviewees", "IdCandidate", "dbo.Candidates");
            DropForeignKey("dbo.CandidateCampaigns", "IdCandidate", "dbo.Candidates");
            DropForeignKey("dbo.Candidates", "Campaign_IdCampaign", "dbo.Campaigns");
            DropIndex("dbo.Candidates", new[] { "Campaign_IdCampaign" });
            DropPrimaryKey("dbo.Candidates");
            AlterColumn("dbo.Candidates", "IdCandidate", c => c.Int(nullable: false));
            DropColumn("dbo.Candidates", "Campaign_IdCampaign");
            AddPrimaryKey("dbo.Candidates", "IdCandidate");
            CreateIndex("dbo.Candidates", "IdCandidate");
            AddForeignKey("dbo.Interviewees", "IdCandidate", "dbo.Candidates", "IdCandidate", cascadeDelete: true);
            AddForeignKey("dbo.CandidateCampaigns", "IdCandidate", "dbo.Candidates", "IdCandidate", cascadeDelete: true);
            AddForeignKey("dbo.Candidates", "IdCandidate", "dbo.Campaigns", "IdCampaign");
        }
    }
}
