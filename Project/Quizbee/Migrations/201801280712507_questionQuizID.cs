namespace Quizbee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class questionQuizID : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "Quiz_ID", "dbo.Quizs");
            DropIndex("dbo.Questions", new[] { "Quiz_ID" });
            RenameColumn(table: "dbo.Questions", name: "Quiz_ID", newName: "QuizID");
            AlterColumn("dbo.Questions", "QuizID", c => c.Int(nullable: false));
            CreateIndex("dbo.Questions", "QuizID");
            AddForeignKey("dbo.Questions", "QuizID", "dbo.Quizs", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "QuizID", "dbo.Quizs");
            DropIndex("dbo.Questions", new[] { "QuizID" });
            AlterColumn("dbo.Questions", "QuizID", c => c.Int());
            RenameColumn(table: "dbo.Questions", name: "QuizID", newName: "Quiz_ID");
            CreateIndex("dbo.Questions", "Quiz_ID");
            AddForeignKey("dbo.Questions", "Quiz_ID", "dbo.Quizs", "ID");
        }
    }
}
