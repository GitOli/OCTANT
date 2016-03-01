namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class candinotmandatory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Candidates", "PhoneNumber", c => c.String());
            AlterColumn("dbo.Candidates", "EmailAddress", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Candidates", "EmailAddress", c => c.String(nullable: false));
            AlterColumn("dbo.Candidates", "PhoneNumber", c => c.String(nullable: false));
        }
    }
}
