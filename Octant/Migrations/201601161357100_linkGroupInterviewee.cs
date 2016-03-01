namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class linkGroupInterviewee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Interviewees", "IdGroup", c => c.Int(nullable: true));
            CreateIndex("dbo.Interviewees", "IdGroup");
            AddForeignKey("dbo.Interviewees", "IdGroup", "dbo.Groups", "IdGroup", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Interviewees", "IdGroup", "dbo.Groups");
            DropIndex("dbo.Interviewees", new[] { "IdGroup" });
            DropColumn("dbo.Interviewees", "IdGroup");
        }
    }
}
