namespace Octant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropdownFix1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ConsultantCampaigns");
            DropPrimaryKey("dbo.CustomScores");
            AddColumn("dbo.Interviews", "Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Frameworks", "Id", c => c.String(maxLength: 128));
            AddColumn("dbo.ConsultantCampaigns", "Id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.CustomScores", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.ConsultantCampaigns", new[] { "Id", "IdCampaign" });
            AddPrimaryKey("dbo.CustomScores", new[] { "Id", "IdNode" });
            CreateIndex("dbo.Interviews", "Id");
            CreateIndex("dbo.Frameworks", "Id");
            CreateIndex("dbo.ConsultantCampaigns", "Id");
            CreateIndex("dbo.CustomScores", "Id");
            AddForeignKey("dbo.Interviews", "Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Frameworks", "Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ConsultantCampaigns", "Id", "dbo.AspNetUsers", "Id", cascadeDelete: false);
            AddForeignKey("dbo.CustomScores", "Id", "dbo.AspNetUsers", "Id", cascadeDelete: false);
            DropColumn("dbo.ConsultantCampaigns", "IdUser");
            DropColumn("dbo.CustomScores", "IdConsultant");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomScores", "IdConsultant", c => c.Int(nullable: false));
            AddColumn("dbo.ConsultantCampaigns", "IdUser", c => c.Int(nullable: false));
            DropForeignKey("dbo.CustomScores", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ConsultantCampaigns", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Frameworks", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Interviews", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.CustomScores", new[] { "Id" });
            DropIndex("dbo.ConsultantCampaigns", new[] { "Id" });
            DropIndex("dbo.Frameworks", new[] { "Id" });
            DropIndex("dbo.Interviews", new[] { "Id" });
            DropPrimaryKey("dbo.CustomScores");
            DropPrimaryKey("dbo.ConsultantCampaigns");
            DropColumn("dbo.CustomScores", "Id");
            DropColumn("dbo.ConsultantCampaigns", "Id");
            DropColumn("dbo.Frameworks", "Id");
            DropColumn("dbo.Interviews", "Id");
            AddPrimaryKey("dbo.CustomScores", new[] { "IdConsultant", "IdNode" });
            AddPrimaryKey("dbo.ConsultantCampaigns", new[] { "IdUser", "IdCampaign" });
        }
    }
}
