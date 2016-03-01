namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edituser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EditUserViewModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IdFirm = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Firms", t => t.IdFirm)
                .Index(t => t.IdFirm);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EditUserViewModels", "IdFirm", "dbo.Firms");
            DropIndex("dbo.EditUserViewModels", new[] { "IdFirm" });
            DropTable("dbo.EditUserViewModels");
        }
    }
}
