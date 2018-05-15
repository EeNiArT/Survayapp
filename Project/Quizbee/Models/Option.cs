using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizbee.Models
{
	public class Option : BaseModel
	{
		public string Answer { get; set; }
		public bool IsCorrect { get; set; }
	}
}