namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Campaigns", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Campaigns", "StartDate", c => c.DateTime());
            AlterColumn("dbo.Campaigns", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Campaigns", "Coverage", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Campaigns", "Coverage", c => c.Int(nullable: false));
            AlterColumn("dbo.Campaigns", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Campaigns", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Campaigns", "Name", c => c.String());
        }
    }
}
