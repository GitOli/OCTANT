namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class required1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Candidates", "PhoneNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Candidates", "PhoneNumber", c => c.String());
        }
    }
}
