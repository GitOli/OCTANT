namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CandidateCampaigns",
                c => new
                {
                    IdCandidate = c.Int(nullable: false),
                    IdCampaign = c.Int(nullable: false),
                    Comment = c.String(),
                })
                .PrimaryKey(t => new { t.IdCandidate, t.IdCampaign })
                .ForeignKey("dbo.Campaigns", t => t.IdCampaign, cascadeDelete: false)
                .ForeignKey("dbo.Candidates", t => t.IdCandidate, cascadeDelete: false)
                .Index(t => t.IdCampaign)
                .Index(t => t.IdCandidate);
        }

        public override void Down()
        {
            DropForeignKey("dbo.CandidateCampaigns", "IdCampaign", "dbo.Campaigns");
            DropForeignKey("dbo.CandidateCampaigns", "IdCandidate", "dbo.Candidates");
            DropIndex("dbo.CandidateCampaigns", new[] { "IdCampaign" });
            DropIndex("dbo.CandidateCampaigns", new[] { "IdCandidate" });
            DropTable("dbo.CandidateCampaigns");
        }
    }
}
