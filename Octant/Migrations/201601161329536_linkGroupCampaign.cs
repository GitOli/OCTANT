namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class linkGroupCampaign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "IdCampaign", c => c.Int(nullable: true));
            CreateIndex("dbo.Groups", "IdCampaign");
            AddForeignKey("dbo.Groups", "IdCampaign", "dbo.Campaigns", "IdCampaign", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "IdCampaign", "dbo.Campaigns");
            DropIndex("dbo.Groups", new[] { "IdCampaign" });
            DropColumn("dbo.Groups", "IdCampaign");
        }
    }
}
