namespace Quizbee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class survey : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Surveys",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        TimeDuration = c.Time(nullable: false, precision: 7),
                        IsActive = c.Boolean(nullable: false),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Questions", "Survey_ID", c => c.Int());
            CreateIndex("dbo.Questions", "Survey_ID");
            AddForeignKey("dbo.Questions", "Survey_ID", "dbo.Surveys", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "Survey_ID", "dbo.Surveys");
            DropIndex("dbo.Questions", new[] { "Survey_ID" });
            DropColumn("dbo.Questions", "Survey_ID");
            DropTable("dbo.Surveys");
        }
    }
}
