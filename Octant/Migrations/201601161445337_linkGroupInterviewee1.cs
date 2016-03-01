namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class linkGroupInterviewee1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Groups", "IdCampaign", "dbo.Campaigns");
            DropIndex("dbo.Groups", new[] { "IdCampaign" });
            AlterColumn("dbo.Groups", "IdCampaign", c => c.Int());
            CreateIndex("dbo.Groups", "IdCampaign");
            AddForeignKey("dbo.Groups", "IdCampaign", "dbo.Campaigns", "IdCampaign");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "IdCampaign", "dbo.Campaigns");
            DropIndex("dbo.Groups", new[] { "IdCampaign" });
            AlterColumn("dbo.Groups", "IdCampaign", c => c.Int(nullable: false));
            CreateIndex("dbo.Groups", "IdCampaign");
            AddForeignKey("dbo.Groups", "IdCampaign", "dbo.Campaigns", "IdCampaign", cascadeDelete: true);
        }
    }
}
