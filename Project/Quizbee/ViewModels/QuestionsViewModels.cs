using Quizbee.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quizbee.ViewModels
{
	public class QuestionListingViewModel : ListingBaseViewModel
	{
		public int QuizID { get; set; }
		public List<Question> Questions { get; set; }
	}

	public class QuestionViewModel : BaseViewModel
	{
		public int ID { get; set; }

		[Required]
		public string Title { get; set; }
		
		public List<Option> Options { get; set; }
		public Option CorrectOption { get; set; }
		
		public int QuizID { get; set; }		
	}
}