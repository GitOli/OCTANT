namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class autoscore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questionnaires", "AutoScore", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questionnaires", "AutoScore");
        }
    }
}
