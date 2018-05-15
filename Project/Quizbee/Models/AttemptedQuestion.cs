using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizbee.Models
{
	public class AttemptedQuestion : BaseModel
	{
		public virtual Question Question { get; set; }

		public virtual Option SelectedOption { get; set; }

		public Nullable<DateTime> AnsweredAt { get; set; }

		public bool IsCorrect { get; set; }
	}
}