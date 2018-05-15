using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizbee.Commons
{
	/// <summary>
	/// I dont want to create enums with integer value, because this is something i want to change in URLs later. So I added these string constants as Operations.
	/// </summary>
	public static class Operations
	{
		/// <summary>
		/// Create a New Record
		/// </summary>
		public static String Create { get { return "new"; } }

		/// <summary>
		/// Modify This Record
		/// </summary>
		public static String Modify { get { return "modify"; } }

		/// <summary>
		/// Update This Record
		/// </summary>
		public static String Update { get { return "update"; } }

		/// <summary>
		/// Delete This Record.
		/// </summary>
		public static String Delete { get { return "delete"; } }
	}

	public static class Variables
	{
	}
}