namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class autoinc3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Campaigns", "IdCustomer", "dbo.Customers");
            DropIndex("dbo.Campaigns", new[] { "IdCustomer" });
            AddColumn("dbo.Campaigns", "Customer_IdCustomer", c => c.Int());
            AddColumn("dbo.Customers", "Campaign_IdCampaign", c => c.Int());
            CreateIndex("dbo.Campaigns", "Customer_IdCustomer");
            CreateIndex("dbo.Customers", "Campaign_IdCampaign");
            AddForeignKey("dbo.Customers", "Campaign_IdCampaign", "dbo.Campaigns", "IdCampaign");
            AddForeignKey("dbo.Campaigns", "Customer_IdCustomer", "dbo.Customers", "IdCustomer");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Campaigns", "Customer_IdCustomer", "dbo.Customers");
            DropForeignKey("dbo.Customers", "Campaign_IdCampaign", "dbo.Campaigns");
            DropIndex("dbo.Customers", new[] { "Campaign_IdCampaign" });
            DropIndex("dbo.Campaigns", new[] { "Customer_IdCustomer" });
            DropColumn("dbo.Customers", "Campaign_IdCampaign");
            DropColumn("dbo.Campaigns", "Customer_IdCustomer");
            CreateIndex("dbo.Campaigns", "IdCustomer");
            AddForeignKey("dbo.Campaigns", "IdCustomer", "dbo.Customers", "IdCustomer", cascadeDelete: true);
        }
    }
}
