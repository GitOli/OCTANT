namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class questionnaire : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Interviews", "IdQuestionnaire", "dbo.Questionnaires");
            DropIndex("dbo.Interviews", new[] { "IdQuestionnaire" });
            AddColumn("dbo.Campaigns", "IdQuestionnaire", c => c.Int(nullable: false));
            CreateIndex("dbo.Campaigns", "IdQuestionnaire");
            AddForeignKey("dbo.Campaigns", "IdQuestionnaire", "dbo.Questionnaires", "IdQuestionnaire", cascadeDelete: true);
            DropColumn("dbo.Interviews", "IdQuestionnaire");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Interviews", "IdQuestionnaire", c => c.Int(nullable: false));
            DropForeignKey("dbo.Campaigns", "IdQuestionnaire", "dbo.Questionnaires");
            DropIndex("dbo.Campaigns", new[] { "IdQuestionnaire" });
            DropColumn("dbo.Campaigns", "IdQuestionnaire");
            CreateIndex("dbo.Interviews", "IdQuestionnaire");
            AddForeignKey("dbo.Interviews", "IdQuestionnaire", "dbo.Questionnaires", "IdQuestionnaire", cascadeDelete: true);
        }
    }
}
