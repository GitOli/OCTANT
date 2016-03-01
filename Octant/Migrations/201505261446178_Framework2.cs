namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Framework2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "IntervieweeAnswer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Answers", "IntervieweeAnswer");
        }
    }
}
