using Quizbee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizbee.ViewModels
{
	public class AttemptQuizViewModel
	{
		public Question Question { get; set; }
		public List<Option> Options { get; set; }

		public int StudentQuizID { get; set; }
		public int QuizID { get; set; }
		public int QuestionID { get; set; }
		public int SelectedOptionID { get; set; }

		public bool TimerExpired { get; set; }

		public int TotalQuestions { get; set; }
		public int QuestionIndex { get; set; }
	}
}