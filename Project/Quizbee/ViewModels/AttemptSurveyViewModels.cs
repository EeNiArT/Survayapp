using Quizbee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizbee.ViewModels
{
    public class AttemptSurveyViewModel
    {
        public SQuestion SQuestion { get; set; }
        public List<Option> Options { get; set; }

        public int StudentSurveyID { get; set; }
        public int SurveyID { get; set; }
        public int QuestionID { get; set; }
        public int SelectedOptionID { get; set; }
        public int AnswerType { get; set; }
        public string AnswerValue { get; set; }
        public bool TimerExpired { get; set; }

        public int TotalQuestions { get; set; }
        public int QuestionIndex { get; set; }
    }
}