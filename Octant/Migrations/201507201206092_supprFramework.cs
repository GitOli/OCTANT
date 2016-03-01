namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class supprFramework : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Campaigns", "IdFramework", "dbo.Frameworks");
            DropIndex("dbo.Campaigns", new[] { "IdFramework" });
            DropColumn("dbo.Campaigns", "IdFramework");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Campaigns", "IdFramework", c => c.Int(nullable: false));
            CreateIndex("dbo.Campaigns", "IdFramework");
            AddForeignKey("dbo.Campaigns", "IdFramework", "dbo.Frameworks", "IdFramework", cascadeDelete: true);
        }
    }
}
