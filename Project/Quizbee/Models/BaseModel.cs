using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizbee.Models
{
	public abstract class BaseModel
	{
		public int ID { get; set; }

		public bool IsActive { get; set; }
		public Nullable<DateTime> ModifiedOn { get; set; }
	}
}