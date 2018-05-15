namespace Quizbee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentIDString : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.StudentQuizs", name: "Student_Id", newName: "StudentID");
            RenameIndex(table: "dbo.StudentQuizs", name: "IX_Student_Id", newName: "IX_StudentID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.StudentQuizs", name: "IX_StudentID", newName: "IX_Student_Id");
            RenameColumn(table: "dbo.StudentQuizs", name: "StudentID", newName: "Student_Id");
        }
    }
}
