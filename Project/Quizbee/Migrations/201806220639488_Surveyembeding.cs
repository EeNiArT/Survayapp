namespace Quizbee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Surveyembeding : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentSurveys",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StudentID = c.String(maxLength: 128),
                        StartedAt = c.DateTime(),
                        CompletedAt = c.DateTime(),
                        Score = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        ModifiedOn = c.DateTime(),
                        Quiz_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Surveys", t => t.Quiz_ID)
                .ForeignKey("dbo.AspNetUsers", t => t.StudentID)
                .Index(t => t.StudentID)
                .Index(t => t.Quiz_ID);
            
            CreateTable(
                "dbo.AttemptedSQuestions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AnsweredAt = c.DateTime(),
                        IsCorrect = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        ModifiedOn = c.DateTime(),
                        Question_ID = c.Int(),
                        SelectedOption_ID = c.Int(),
                        StudentSurvey_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SQuestions", t => t.Question_ID)
                .ForeignKey("dbo.Options", t => t.SelectedOption_ID)
                .ForeignKey("dbo.StudentSurveys", t => t.StudentSurvey_ID)
                .Index(t => t.Question_ID)
                .Index(t => t.SelectedOption_ID)
                .Index(t => t.StudentSurvey_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentSurveys", "StudentID", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentSurveys", "Quiz_ID", "dbo.Surveys");
            DropForeignKey("dbo.AttemptedSQuestions", "StudentSurvey_ID", "dbo.StudentSurveys");
            DropForeignKey("dbo.AttemptedSQuestions", "SelectedOption_ID", "dbo.Options");
            DropForeignKey("dbo.AttemptedSQuestions", "Question_ID", "dbo.SQuestions");
            DropIndex("dbo.AttemptedSQuestions", new[] { "StudentSurvey_ID" });
            DropIndex("dbo.AttemptedSQuestions", new[] { "SelectedOption_ID" });
            DropIndex("dbo.AttemptedSQuestions", new[] { "Question_ID" });
            DropIndex("dbo.StudentSurveys", new[] { "Quiz_ID" });
            DropIndex("dbo.StudentSurveys", new[] { "StudentID" });
            DropTable("dbo.AttemptedSQuestions");
            DropTable("dbo.StudentSurveys");
        }
    }
}
