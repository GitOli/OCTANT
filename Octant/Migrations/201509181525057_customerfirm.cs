namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class customerfirm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "IdFirm", c => c.Int(nullable: true));
            CreateIndex("dbo.Customers", "IdFirm");
            AddForeignKey("dbo.Customers", "IdFirm", "dbo.Firms", "IdFirm");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "IdFirm", "dbo.Firms");
            DropIndex("dbo.Customers", new[] { "IdFirm" });
            DropColumn("dbo.Customers", "IdFirm");
        }
    }
}
