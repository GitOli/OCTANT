namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class autoinc : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CandidateCampaigns", "IdCandidate", "dbo.Candidates");
            DropForeignKey("dbo.Interviewees", "IdCandidate", "dbo.Candidates");
            DropIndex("dbo.Candidates", new[] { "IdCandidate" });
            DropPrimaryKey("dbo.Candidates");
            AlterColumn("dbo.Candidates", "IdCandidate", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Candidates", "IdCandidate");
            CreateIndex("dbo.Candidates", "IdCandidate");
            AddForeignKey("dbo.CandidateCampaigns", "IdCandidate", "dbo.Candidates", "IdCandidate", cascadeDelete: true);
            AddForeignKey("dbo.Interviewees", "IdCandidate", "dbo.Candidates", "IdCandidate", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Interviewees", "IdCandidate", "dbo.Candidates");
            DropForeignKey("dbo.CandidateCampaigns", "IdCandidate", "dbo.Candidates");
            DropIndex("dbo.Candidates", new[] { "IdCandidate" });
            DropPrimaryKey("dbo.Candidates");
            AlterColumn("dbo.Candidates", "IdCandidate", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Candidates", "IdCandidate");
            CreateIndex("dbo.Candidates", "IdCandidate");
            AddForeignKey("dbo.Interviewees", "IdCandidate", "dbo.Candidates", "IdCandidate", cascadeDelete: true);
            AddForeignKey("dbo.CandidateCampaigns", "IdCandidate", "dbo.Candidates", "IdCandidate", cascadeDelete: true);
        }
    }
}
