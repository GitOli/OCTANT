namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Firms",
                c => new
                    {
                        IdFirm = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.IdFirm);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Firms");
        }
    }
}
