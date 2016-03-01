namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edituser1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Firms", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Firms", "Deleted");
        }
    }
}
