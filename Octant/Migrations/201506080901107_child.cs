namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class child : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Nodes", "IdChild", c => c.Int());
            CreateIndex("dbo.Nodes", "IdChild");
            AddForeignKey("dbo.Nodes", "IdChild", "dbo.Nodes", "IdNode");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Nodes", "IdChild", "dbo.Nodes");
            DropIndex("dbo.Nodes", new[] { "IdChild" });
            DropColumn("dbo.Nodes", "IdChild");
        }
    }
}
