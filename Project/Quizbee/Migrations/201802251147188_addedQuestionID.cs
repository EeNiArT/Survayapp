namespace Quizbee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedQuestionID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Options", "QuestionID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Options", "QuestionID");
        }
    }
}
