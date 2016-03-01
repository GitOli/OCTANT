namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Kappa3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "Campaign_IdCampaign", "dbo.Campaigns");
            DropForeignKey("dbo.Customers", "Campaign_IdCampaign1", "dbo.Campaigns");
            DropForeignKey("dbo.Campaigns", "Customer_IdCustomer", "dbo.Customers");
            DropIndex("dbo.Campaigns", new[] { "Customer_IdCustomer" });
            DropIndex("dbo.Customers", new[] { "Campaign_IdCampaign" });
            DropIndex("dbo.Customers", new[] { "Campaign_IdCampaign1" });
            AddColumn("dbo.Campaigns", "IdCustomer", c => c.Int());
            //RenameColumn(table: "dbo.Campaigns", name: "Customer_IdCustomer", newName: "IdCustomer");
            //AlterColumn("dbo.Campaigns", "IdCustomer", c => c.Int(nullable: false));
            CreateIndex("dbo.Campaigns", "IdCustomer");
            AddForeignKey("dbo.Campaigns", "IdCustomer", "dbo.Customers", "IdCustomer", cascadeDelete: true);
            DropColumn("dbo.Customers", "Campaign_IdCampaign");
            DropColumn("dbo.Customers", "Campaign_IdCampaign1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "Campaign_IdCampaign1", c => c.Int());
            AddColumn("dbo.Customers", "Campaign_IdCampaign", c => c.Int());
            DropForeignKey("dbo.Campaigns", "IdCustomer", "dbo.Customers");
            DropIndex("dbo.Campaigns", new[] { "IdCustomer" });
            AlterColumn("dbo.Campaigns", "IdCustomer", c => c.Int());
            RenameColumn(table: "dbo.Campaigns", name: "IdCustomer", newName: "Customer_IdCustomer");
            AddColumn("dbo.Campaigns", "IdCustomer", c => c.Int(nullable: false));
            CreateIndex("dbo.Customers", "Campaign_IdCampaign1");
            CreateIndex("dbo.Customers", "Campaign_IdCampaign");
            CreateIndex("dbo.Campaigns", "Customer_IdCustomer");
            AddForeignKey("dbo.Campaigns", "Customer_IdCustomer", "dbo.Customers", "IdCustomer");
            AddForeignKey("dbo.Customers", "Campaign_IdCampaign1", "dbo.Campaigns", "IdCampaign");
            AddForeignKey("dbo.Customers", "Campaign_IdCampaign", "dbo.Campaigns", "IdCampaign");
        }
    }
}
