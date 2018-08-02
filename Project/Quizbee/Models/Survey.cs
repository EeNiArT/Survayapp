using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizbee.Models
{
    public class Survey : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string QFMen { get; set; }
        public string QFWomen { get; set; }
        public TimeSpan TimeDuration { get; set; }
        public DateTime ExpireOn { get; set; }
        public virtual List<SQuestion> SQuestions { get; set; }
    }
}