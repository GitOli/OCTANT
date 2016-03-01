namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class email : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Candidates", "EmailAddress", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Candidates", "EmailAddress", c => c.Int(nullable: false));
        }
    }
}
