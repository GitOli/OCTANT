namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idnotrequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Interviews", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Interviews", new[] { "Id" });
            AlterColumn("dbo.Interviews", "Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Interviews", "Id");
            AddForeignKey("dbo.Interviews", "Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Interviews", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Interviews", new[] { "Id" });
            AlterColumn("dbo.Interviews", "Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Interviews", "Id");
            AddForeignKey("dbo.Interviews", "Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
