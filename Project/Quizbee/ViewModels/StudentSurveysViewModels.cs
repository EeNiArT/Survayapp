using Quizbee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizbee.ViewModels
{
    public class StudentSurveyViewModel : BaseViewModel
    {
        public StudentSurvey StudentSurvey { get; set; }

        public bool TimerExpired { get; set; }

        public int Hours
        {
            get
            {
                if (StudentSurvey.StartedAt.HasValue && StudentSurvey.CompletedAt.HasValue)
                    return (StudentSurvey.CompletedAt.Value - StudentSurvey.StartedAt.Value).Hours;
                else return 0;
            }
        }
        public int Minutes
        {
            get
            {
                if (StudentSurvey.StartedAt.HasValue && StudentSurvey.CompletedAt.HasValue)
                    return (StudentSurvey.CompletedAt.Value - StudentSurvey.StartedAt.Value).Minutes;
                else return 0;
            }
        }
        public int Seconds
        {
            get
            {
                if (StudentSurvey.StartedAt.HasValue && StudentSurvey.CompletedAt.HasValue)
                    return (StudentSurvey.CompletedAt.Value - StudentSurvey.StartedAt.Value).Seconds;
                else return 0;
            }
        }
    }

    public class StudentSurveyListViewModel : ListingBaseViewModel
    {
        public bool isPartialView { get; set; }

        public List<StudentSurvey> StudentSurveys { get; set; }
    }
}