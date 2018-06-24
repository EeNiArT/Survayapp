using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizbee.Models
{
    public class StudentSurvey : BaseModel
    {
        public string StudentID { get; set; }
        public virtual ApplicationUser Student { get; set; }

        public virtual Survey Survey { get; set; }

        public Nullable<DateTime> StartedAt { get; set; }
        public Nullable<DateTime> CompletedAt { get; set; }

        public virtual List<AttemptedSQuestion> AttemptedSQuestions { get; set; }

        public int Score { get; set; }
    }
}