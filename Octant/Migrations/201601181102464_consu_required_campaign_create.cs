namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class consu_required_campaign_create : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Campaigns", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Campaigns", new[] { "Id" });
            AlterColumn("dbo.Campaigns", "Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Campaigns", "Id");
            AddForeignKey("dbo.Campaigns", "Id", "dbo.AspNetUsers", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Campaigns", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Campaigns", new[] { "Id" });
            AlterColumn("dbo.Campaigns", "Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Campaigns", "Id");
            AddForeignKey("dbo.Campaigns", "Id", "dbo.AspNetUsers", "Id");
        }
    }
}
