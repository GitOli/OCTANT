namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "Id", c => c.String(maxLength: 128));
            AddForeignKey("dbo.Campaigns", "Id", "dbo.AspNetUsers", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
        }
    }
}
