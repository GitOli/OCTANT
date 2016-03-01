namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Ident", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "Ident");
        }
    }
}
