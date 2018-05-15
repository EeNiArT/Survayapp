namespace Quizbee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedQuestionToOption : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Options", "Question_ID1", c => c.Int());
            CreateIndex("dbo.Options", "Question_ID1");
            AddForeignKey("dbo.Options", "Question_ID1", "dbo.Questions", "ID");
            DropColumn("dbo.Options", "QuestionID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Options", "QuestionID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Options", "Question_ID1", "dbo.Questions");
            DropIndex("dbo.Options", new[] { "Question_ID1" });
            DropColumn("dbo.Options", "Question_ID1");
        }
    }
}
