namespace Quizbee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class attemptedTimes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttemptedQuestions", "AnsweredAt", c => c.DateTime());
            AddColumn("dbo.StudentQuizs", "StartedAt", c => c.DateTime());
            AddColumn("dbo.StudentQuizs", "CompletedAt", c => c.DateTime());
            DropColumn("dbo.StudentQuizs", "AttemptedOn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StudentQuizs", "AttemptedOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.StudentQuizs", "CompletedAt");
            DropColumn("dbo.StudentQuizs", "StartedAt");
            DropColumn("dbo.AttemptedQuestions", "AnsweredAt");
        }
    }
}
