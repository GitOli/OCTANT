namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Kappa2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "Campaign_IdCampaign", "dbo.Campaigns");
            AddColumn("dbo.Customers", "Campaign_IdCampaign1", c => c.Int());
            CreateIndex("dbo.Customers", "Campaign_IdCampaign1");
            AddForeignKey("dbo.Customers", "Campaign_IdCampaign", "dbo.Campaigns", "IdCampaign");
            AddForeignKey("dbo.Customers", "Campaign_IdCampaign1", "dbo.Campaigns", "IdCampaign");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "Campaign_IdCampaign1", "dbo.Campaigns");
            DropForeignKey("dbo.Customers", "Campaign_IdCampaign", "dbo.Campaigns");
            DropIndex("dbo.Customers", new[] { "Campaign_IdCampaign1" });
            DropColumn("dbo.Customers", "Campaign_IdCampaign1");
            AddForeignKey("dbo.Customers", "Campaign_IdCampaign", "dbo.Campaigns", "IdCampaign");
        }
    }
}
