namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bool1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Questionnaires", "AutoScore", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questionnaires", "AutoScore", c => c.Boolean());
        }
    }
}
