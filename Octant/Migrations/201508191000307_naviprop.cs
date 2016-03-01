namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class naviprop : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Candidates", "IdCandidate", "dbo.Interviews");
            AddColumn("dbo.Candidates", "Interview_IdInterview", c => c.Int());
            CreateIndex("dbo.Candidates", "Interview_IdInterview");
            AddForeignKey("dbo.Candidates", "Interview_IdInterview", "dbo.Interviews", "IdInterview");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Candidates", "Interview_IdInterview", "dbo.Interviews");
            DropIndex("dbo.Candidates", new[] { "Interview_IdInterview" });
            DropColumn("dbo.Candidates", "Interview_IdInterview");
            //AddForeignKey("dbo.Candidates", "IdCandidate", "dbo.Interviews", "IdInterview");
        }
    }
}
