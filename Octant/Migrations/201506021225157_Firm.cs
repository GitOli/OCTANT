namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Firm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IdFirm", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "IdFirm");
            AddForeignKey("dbo.AspNetUsers", "IdFirm", "dbo.Firms", "IdFirm");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "IdFirm", "dbo.Firms");
            DropIndex("dbo.AspNetUsers", new[] { "IdFirm" });
            DropColumn("dbo.AspNetUsers", "IdFirm");
        }
    }
}
