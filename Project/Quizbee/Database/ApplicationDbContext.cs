using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Quizbee.Models;

namespace Quizbee.Database
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext()
			: base("QuizbeeConnection", throwIfV1Schema: false)
		{
		}

		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}

		public DbSet<Quiz> Quizzes { get; set; }
		public DbSet<Question> Questions { get; set; }
        public DbSet<SQuestion> SQuestions { get; set; }
        public DbSet<Option> Options { get; set; }
		public DbSet<StudentQuiz> StudentQuizzes { get; set; }
        public DbSet<StudentSurvey> StudentSurveys { get; set; }
        public DbSet<AttemptedQuestion> AttemptedQuestions { get; set; }
        public DbSet<AttemptedSQuestion> AttemptedSQuestions { get; set; }
        public DbSet<Survey> Surveys { get; set; }

	}
}