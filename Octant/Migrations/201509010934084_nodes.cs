namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nodes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Nodes", "IdQuestionnaire", c => c.Int());
            CreateIndex("dbo.Nodes", "IdQuestionnaire");
            AddForeignKey("dbo.Nodes", "IdQuestionnaire", "dbo.Questionnaires", "IdQuestionnaire");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Nodes", "IdQuestionnaire", "dbo.Questionnaires");
            DropIndex("dbo.Nodes", new[] { "IdQuestionnaire" });
            DropColumn("dbo.Nodes", "IdQuestionnaire");
        }
    }
}
