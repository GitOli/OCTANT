namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class autoinc5 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CandidateCampaigns");
            AddPrimaryKey("dbo.CandidateCampaigns", new[] { "IdCandidate", "IdCampaign" });
            DropColumn("dbo.CandidateCampaigns", "IdCandidateCampaign");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CandidateCampaigns", "IdCandidateCampaign", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.CandidateCampaigns");
            AddPrimaryKey("dbo.CandidateCampaigns", "IdCandidateCampaign");
        }
    }
}
