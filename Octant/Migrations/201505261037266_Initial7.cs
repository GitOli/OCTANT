namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Campaigns", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Campaigns", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Campaigns", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Campaigns", "StartDate", c => c.DateTime());
        }
    }
}
