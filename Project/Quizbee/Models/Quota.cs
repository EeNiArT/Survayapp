using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizbee.Models
{
    public class Quota : BaseModel
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string ByGender { get; set; }
        public string Byage { get; set; }
        public string ByCity { get; set; }
        public DateTime CreatedOn { get; set; }
        

    }
}