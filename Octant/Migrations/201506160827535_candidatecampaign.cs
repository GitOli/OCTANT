namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class candidatecampaign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Campaign_IdCampaign", c => c.Int());
            AddColumn("dbo.Candidates", "Interview_IdInterview", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Campaign_IdCampaign");
            CreateIndex("dbo.Candidates", "Interview_IdInterview");
            AddForeignKey("dbo.AspNetUsers", "Campaign_IdCampaign", "dbo.Campaigns", "IdCampaign");
            AddForeignKey("dbo.Candidates", "Interview_IdInterview", "dbo.Interviews", "IdInterview");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Candidates", "Interview_IdInterview", "dbo.Interviews");
            DropForeignKey("dbo.AspNetUsers", "Campaign_IdCampaign", "dbo.Campaigns");
            DropIndex("dbo.Candidates", new[] { "Interview_IdInterview" });
            DropIndex("dbo.AspNetUsers", new[] { "Campaign_IdCampaign" });
            DropColumn("dbo.Candidates", "Interview_IdInterview");
            DropColumn("dbo.AspNetUsers", "Campaign_IdCampaign");
        }
    }
}
