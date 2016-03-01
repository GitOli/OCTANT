namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class softdelete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Interviews", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Campaigns", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaigns", "Deleted");
            DropColumn("dbo.Interviews", "Deleted");
        }
    }
}
