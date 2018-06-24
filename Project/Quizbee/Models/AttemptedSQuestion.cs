using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizbee.Models
{
    public class AttemptedSQuestion : BaseModel
    {
        public virtual SQuestion SQuestion { get; set; }

        public virtual Option SelectedOption { get; set; }

        public virtual string AnswerValue { get; set; }

        public Nullable<DateTime> AnsweredAt { get; set; }

        public bool IsCorrect { get; set; }
    }
}