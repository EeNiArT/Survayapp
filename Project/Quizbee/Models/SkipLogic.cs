using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizbee.Models
{
    public class SkipLogic : BaseModel
    {
        public virtual SQuestion S_Question { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public bool SkipSurvey { get; set; }
        public virtual Option Option { get; set; } 
        public string SkipSQuestionsIDs { get; set; }
        public DateTime CreatedOn { get; set; }
        
    }
}