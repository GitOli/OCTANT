namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class possibleanswer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "PossibleAnswer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "PossibleAnswer");
        }
    }
}
