namespace Quizbee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isActivecheck : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttemptedQuestions", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Questions", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Options", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Quizs", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.StudentQuizs", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentQuizs", "IsActive");
            DropColumn("dbo.Quizs", "IsActive");
            DropColumn("dbo.Options", "IsActive");
            DropColumn("dbo.Questions", "IsActive");
            DropColumn("dbo.AttemptedQuestions", "IsActive");
        }
    }
}
