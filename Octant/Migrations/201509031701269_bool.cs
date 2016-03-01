namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _bool : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Questionnaires", "AutoScore", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questionnaires", "AutoScore", c => c.Boolean(nullable: false));
        }
    }
}
