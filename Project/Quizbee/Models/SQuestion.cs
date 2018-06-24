using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizbee.Models
{
    public class SQuestion : BaseModel
    {
        public string Title { get; set; }
        public virtual List<Option> Options { get; set; }
        public int SurveyID { get; set; }

        public int TypeOfQuestion { get; set; }
        public bool IsMale { get; set; }

        public bool IsQuiz { get; set; }
    }
}