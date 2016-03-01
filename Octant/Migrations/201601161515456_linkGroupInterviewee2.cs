namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class linkGroupInterviewee2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Interviewees", "IdGroup", "dbo.Groups");
            DropIndex("dbo.Interviewees", new[] { "IdGroup" });
            AlterColumn("dbo.Interviewees", "IdGroup", c => c.Int());
            CreateIndex("dbo.Interviewees", "IdGroup");
            AddForeignKey("dbo.Interviewees", "IdGroup", "dbo.Groups", "IdGroup");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Interviewees", "IdGroup", "dbo.Groups");
            DropIndex("dbo.Interviewees", new[] { "IdGroup" });
            AlterColumn("dbo.Interviewees", "IdGroup", c => c.Int(nullable: false));
            CreateIndex("dbo.Interviewees", "IdGroup");
            AddForeignKey("dbo.Interviewees", "IdGroup", "dbo.Groups", "IdGroup", cascadeDelete: true);
        }
    }
}
