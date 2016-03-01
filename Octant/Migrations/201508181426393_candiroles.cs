namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class candiroles : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Candidates", "Group_IdGroup", "dbo.Groups");
            //DropForeignKey("dbo.Candidates", "Group_IdGroup1", "dbo.Groups");
            //DropForeignKey("dbo.Candidates", "Role_IdGroup", "dbo.Groups");
            //DropIndex("dbo.Candidates", new[] { "Group_IdGroup" });
            //DropIndex("dbo.Candidates", new[] { "Group_IdGroup1" });
            //DropIndex("dbo.Candidates", new[] { "Role_IdGroup" });
            //DropColumn("dbo.Candidates", "IdGroup");
            //DropColumn("dbo.Candidates", "IdRole");
            //RenameColumn(table: "dbo.Candidates", name: "Group_IdGroup1", newName: "IdGroup");
            //RenameColumn(table: "dbo.Candidates", name: "Role_IdGroup", newName: "IdRole");
            AlterColumn("dbo.Candidates", "IdGroup", c => c.Int());
            AlterColumn("dbo.Candidates", "IdRole", c => c.Int());
            CreateIndex("dbo.Candidates", "IdGroup");
            CreateIndex("dbo.Candidates", "IdRole");
            AddForeignKey("dbo.Candidates", "IdGroup", "dbo.Groups", "IdGroup");
            AddForeignKey("dbo.Candidates", "IdRole", "dbo.Roles", "IdRole");
            //DropColumn("dbo.Candidates", "Group_IdGroup");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Candidates", "Group_IdGroup", c => c.Int());
            DropForeignKey("dbo.Candidates", "IdRole", "dbo.Roles");
            DropForeignKey("dbo.Candidates", "IdGroup", "dbo.Groups");
            DropIndex("dbo.Candidates", new[] { "IdRole" });
            DropIndex("dbo.Candidates", new[] { "IdGroup" });
            AlterColumn("dbo.Candidates", "IdRole", c => c.Int());
            AlterColumn("dbo.Candidates", "IdGroup", c => c.Int());
            RenameColumn(table: "dbo.Candidates", name: "IdRole", newName: "Role_IdGroup");
            RenameColumn(table: "dbo.Candidates", name: "IdGroup", newName: "Group_IdGroup1");
            AddColumn("dbo.Candidates", "IdRole", c => c.Int(nullable: false));
            AddColumn("dbo.Candidates", "IdGroup", c => c.Int(nullable: false));
            CreateIndex("dbo.Candidates", "Role_IdGroup");
            CreateIndex("dbo.Candidates", "Group_IdGroup1");
            CreateIndex("dbo.Candidates", "Group_IdGroup");
            AddForeignKey("dbo.Candidates", "Role_IdGroup", "dbo.Groups", "IdGroup");
            AddForeignKey("dbo.Candidates", "Group_IdGroup1", "dbo.Groups", "IdGroup");
            AddForeignKey("dbo.Candidates", "Group_IdGroup", "dbo.Groups", "IdGroup");
        }
    }
}
