using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizbee.Models
{
	public class StudentQuiz : BaseModel
	{
		public string StudentID { get; set; }
		public virtual ApplicationUser Student { get; set; }

		public virtual Quiz Quiz { get; set; }

		public Nullable<DateTime> StartedAt { get; set; }
		public Nullable<DateTime> CompletedAt { get; set; }

		public virtual List<AttemptedQuestion> AttemptedQuestions { get; set; }

		public int Score { get; set; }
	}
}