namespace Quizbee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correctOptionRemovedFromQuestion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "CorrectOption_ID", "dbo.Options");
            DropIndex("dbo.Questions", new[] { "CorrectOption_ID" });
            AddColumn("dbo.Options", "IsCorrect", c => c.Boolean(nullable: false));
            DropColumn("dbo.Questions", "CorrectOption_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "CorrectOption_ID", c => c.Int());
            DropColumn("dbo.Options", "IsCorrect");
            CreateIndex("dbo.Questions", "CorrectOption_ID");
            AddForeignKey("dbo.Questions", "CorrectOption_ID", "dbo.Options", "ID");
        }
    }
}
