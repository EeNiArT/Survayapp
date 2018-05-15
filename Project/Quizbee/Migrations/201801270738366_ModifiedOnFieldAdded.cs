namespace Quizbee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedOnFieldAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Options", "ModifiedOn", c => c.DateTime());
            AddColumn("dbo.Questions", "ModifiedOn", c => c.DateTime());
            AddColumn("dbo.Quizs", "ModifiedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Quizs", "ModifiedOn");
            DropColumn("dbo.Questions", "ModifiedOn");
            DropColumn("dbo.Options", "ModifiedOn");
        }
    }
}
