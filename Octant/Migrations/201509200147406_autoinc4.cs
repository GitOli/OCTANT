namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class autoinc4 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CandidateCampaigns");
            AddColumn("dbo.CandidateCampaigns", "IdCandidateCampaign", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.CandidateCampaigns", "IdCandidateCampaign");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.CandidateCampaigns");
            DropColumn("dbo.CandidateCampaigns", "IdCandidateCampaign");
            AddPrimaryKey("dbo.CandidateCampaigns", new[] { "IdCandidate", "IdCampaign" });
        }
    }
}
