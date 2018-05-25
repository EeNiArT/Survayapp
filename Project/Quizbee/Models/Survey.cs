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
        public TimeSpan TimeDuration { get; set; }
        public virtual List<Question> Questions { get; set; }
    }
}