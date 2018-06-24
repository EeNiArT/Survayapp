using Quizbee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizbee.ViewModels
{
	public class HomeViewModel : BaseViewModel
	{
		public List<Quiz> Quizzes { get; set; }
        public List<Survey> Surveys { get; set; }
    }
}