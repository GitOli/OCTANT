namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class scoreci : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Interviews", "Score", c => c.Int());
            AddColumn("dbo.Campaigns", "Score", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaigns", "Score");
            DropColumn("dbo.Interviews", "Score");
        }
    }
}
