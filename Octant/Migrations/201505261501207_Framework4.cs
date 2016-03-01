namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Framework4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "IdFramework", c => c.Int(nullable: false));
            AlterColumn("dbo.Campaigns", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Campaigns", "EndDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Campaigns", "IdFramework");
            AddForeignKey("dbo.Campaigns", "IdFramework", "dbo.Frameworks", "IdFramework", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Campaigns", "IdFramework", "dbo.Frameworks");
            DropIndex("dbo.Campaigns", new[] { "IdFramework" });
            AlterColumn("dbo.Campaigns", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Campaigns", "StartDate", c => c.DateTime());
            DropColumn("dbo.Campaigns", "IdFramework");
        }
    }
}
