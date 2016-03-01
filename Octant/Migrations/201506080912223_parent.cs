namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class parent : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Nodes", name: "IdChild", newName: "IdParentNode");
            RenameIndex(table: "dbo.Nodes", name: "IX_IdChild", newName: "IX_IdParentNode");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Nodes", name: "IX_IdParentNode", newName: "IX_IdChild");
            RenameColumn(table: "dbo.Nodes", name: "IdParentNode", newName: "IdChild");
        }
    }
}
