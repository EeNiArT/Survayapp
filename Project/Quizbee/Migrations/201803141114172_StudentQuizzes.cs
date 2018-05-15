namespace Quizbee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentQuizzes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Options", "Question_ID1", "dbo.Questions");
            DropIndex("dbo.Options", new[] { "Question_ID1" });
            CreateTable(
                "dbo.AttemptedQuestions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IsCorrect = c.Boolean(nullable: false),
                        ModifiedOn = c.DateTime(),
                        Question_ID = c.Int(),
                        SelectedOption_ID = c.Int(),
                        StudentQuiz_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Questions", t => t.Question_ID)
                .ForeignKey("dbo.Options", t => t.SelectedOption_ID)
                .ForeignKey("dbo.StudentQuizs", t => t.StudentQuiz_ID)
                .Index(t => t.Question_ID)
                .Index(t => t.SelectedOption_ID)
                .Index(t => t.StudentQuiz_ID);
            
            CreateTable(
                "dbo.StudentQuizs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AttemptedOn = c.DateTime(nullable: false),
                        Score = c.Int(nullable: false),
                        ModifiedOn = c.DateTime(),
                        Quiz_ID = c.Int(),
                        Student_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Quizs", t => t.Quiz_ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Student_Id)
                .Index(t => t.Quiz_ID)
                .Index(t => t.Student_Id);
            
            DropColumn("dbo.Options", "Question_ID1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Options", "Question_ID1", c => c.Int());
            DropForeignKey("dbo.StudentQuizs", "Student_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentQuizs", "Quiz_ID", "dbo.Quizs");
            DropForeignKey("dbo.AttemptedQuestions", "StudentQuiz_ID", "dbo.StudentQuizs");
            DropForeignKey("dbo.AttemptedQuestions", "SelectedOption_ID", "dbo.Options");
            DropForeignKey("dbo.AttemptedQuestions", "Question_ID", "dbo.Questions");
            DropIndex("dbo.StudentQuizs", new[] { "Student_Id" });
            DropIndex("dbo.StudentQuizs", new[] { "Quiz_ID" });
            DropIndex("dbo.AttemptedQuestions", new[] { "StudentQuiz_ID" });
            DropIndex("dbo.AttemptedQuestions", new[] { "SelectedOption_ID" });
            DropIndex("dbo.AttemptedQuestions", new[] { "Question_ID" });
            DropTable("dbo.StudentQuizs");
            DropTable("dbo.AttemptedQuestions");
            CreateIndex("dbo.Options", "Question_ID1");
            AddForeignKey("dbo.Options", "Question_ID1", "dbo.Questions", "ID");
        }
    }
}
