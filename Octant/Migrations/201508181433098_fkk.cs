namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fkk : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.CandidateCampaigns", "IdCandidate", "dbo.Candidates");
            //DropForeignKey("dbo.Interviewees", "IdCandidate", "dbo.Candidates");
            //DropIndex("dbo.Candidates", new[] { "Campaign_IdCampaign" });
            //DropIndex("dbo.Candidates", new[] { "Interview_IdInterview" });
            //DropColumn("dbo.Candidates", "IdCandidate");
            //DropColumn("dbo.Candidates", "IdCandidate");
            //RenameColumn(table: "dbo.Candidates", name: "Interview_IdInterview", newName: "IdCandidate");
            //RenameColumn(table: "dbo.Candidates", name: "Campaign_IdCampaign", newName: "IdCandidate");
            //DropPrimaryKey("dbo.Candidates");
            //AlterColumn("dbo.Candidates", "IdCandidate", c => c.Int(nullable: false, identity: true));
            //AlterColumn("dbo.Candidates", "IdCandidate", c => c.Int(nullable: false, identity: true));
            //AddPrimaryKey("dbo.Candidates", "IdCandidate");
            AddColumn("dbo.Candidates", "Interview_IdInterview", c => c.Int());
            CreateIndex("dbo.Candidates", "Interview_IdInterview");
            AddForeignKey("dbo.Candidates", "Interview_IdInterview", "dbo.Interviews", "IdInterview");
            AddColumn("dbo.Candidates", "Campaign_IdCampaign", c => c.Int());
            CreateIndex("dbo.Candidates", "Campaign_IdCampaign");
            AddForeignKey("dbo.Candidates", "Campaign_IdCampaign", "dbo.Campaigns", "IdCampaign");
            //CreateIndex("dbo.Candidates", "IdCandidate");
            //AddForeignKey("dbo.CandidateCampaigns", "IdCandidate", "dbo.Candidates", "IdCandidate");
            //AddForeignKey("dbo.Interviewees", "IdCandidate", "dbo.Candidates", "IdCandidate");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Interviewees", "IdCandidate", "dbo.Candidates");
            DropForeignKey("dbo.CandidateCampaigns", "IdCandidate", "dbo.Candidates");
            DropIndex("dbo.Candidates", new[] { "IdCandidate" });
            DropPrimaryKey("dbo.Candidates");
            AlterColumn("dbo.Candidates", "IdCandidate", c => c.Int());
            AlterColumn("dbo.Candidates", "IdCandidate", c => c.Int());
            AddPrimaryKey("dbo.Candidates", "IdCandidate");
            RenameColumn(table: "dbo.Candidates", name: "IdCandidate", newName: "Campaign_IdCampaign");
            RenameColumn(table: "dbo.Candidates", name: "IdCandidate", newName: "Interview_IdInterview");
            AddColumn("dbo.Candidates", "IdCandidate", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Candidates", "IdCandidate", c => c.Int(nullable: false, identity: true));
            CreateIndex("dbo.Candidates", "Interview_IdInterview");
            CreateIndex("dbo.Candidates", "Campaign_IdCampaign");
            AddForeignKey("dbo.Interviewees", "IdCandidate", "dbo.Candidates", "IdCandidate", cascadeDelete: true);
            AddForeignKey("dbo.CandidateCampaigns", "IdCandidate", "dbo.Candidates", "IdCandidate", cascadeDelete: true);
        }
    }
}
