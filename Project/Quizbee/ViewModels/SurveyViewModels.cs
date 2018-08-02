using Quizbee.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quizbee.ViewModels
{
    public class SurveyListViewModel : ListingBaseViewModel
    {
        public List<Survey> Surveys { get; set; }
    }

    public class SurveyViewModel : BaseViewModel
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(0, 23, ErrorMessage = "Hours should be between 0 to 23")]
        public int Hours { get; set; }
        [Required]
        [Range(0, 59, ErrorMessage = "Minutes should be between 0 to 59")]
        public int Minutes { get; set; }
        public int NoOfQuestions { get; set; }

        public int TypeOfQuestion { get; set; }

        public bool IsMale { get; set; }

        public DateTime ExpireOn { get; set; }

        public string QFMen { get; set; }
        public string QFWomen { get; set; }

        public List<SQuestion> SQuestions { get; set; }
        public TimeSpan TimeDuration { get; internal set; }
    }
}