namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class questionning : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "IdQuestionnaire", c => c.Int(nullable: true));
            CreateIndex("dbo.Questions", "IdQuestionnaire");
            AddForeignKey("dbo.Questions", "IdQuestionnaire", "dbo.Questionnaires", "IdQuestionnaire", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "IdQuestionnaire", "dbo.Questionnaires");
            DropIndex("dbo.Questions", new[] { "IdQuestionnaire" });
            DropColumn("dbo.Questions", "IdQuestionnaire");
        }
    }
}
