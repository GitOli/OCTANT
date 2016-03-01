namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class candifunction : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Candidates", "IdRole", "dbo.Roles");
            DropIndex("dbo.Candidates", new[] { "IdRole" });
            AddColumn("dbo.Candidates", "Function", c => c.String());
            AddColumn("dbo.Candidates", "Comment", c => c.String());
            DropColumn("dbo.Candidates", "IdRole");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Candidates", "IdRole", c => c.Int(nullable: false));
            DropColumn("dbo.Candidates", "Comment");
            DropColumn("dbo.Candidates", "Function");
            CreateIndex("dbo.Candidates", "IdRole");
            AddForeignKey("dbo.Candidates", "IdRole", "dbo.Roles", "IdRole", cascadeDelete: true);
        }
    }
}
