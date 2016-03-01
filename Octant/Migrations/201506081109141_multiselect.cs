namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class multiselect : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidates", "Campaign_IdCampaign", c => c.Int());
            CreateIndex("dbo.Candidates", "Campaign_IdCampaign");
            AddForeignKey("dbo.Candidates", "Campaign_IdCampaign", "dbo.Campaigns", "IdCampaign");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Candidates", "Campaign_IdCampaign", "dbo.Campaigns");
            DropIndex("dbo.Candidates", new[] { "Campaign_IdCampaign" });
            DropColumn("dbo.Candidates", "Campaign_IdCampaign");
        }
    }
}
