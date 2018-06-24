using Quizbee.Database;
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

        public static int GetQuestionType(string value)
        {
            if (value == "mc") { return 1; }
            else if (value == "sc") { return 2; }
            else if (value == "lc") { return 3; }
            else if (value == "oc") { return 4; }
            else { return 0; }
        }
        public static string GetQuestionType(int value)
        {
            if (value == 1) { return "mc"; }
            else if (value == 2) { return "sc"; }
            else if (value == 3) { return "lc"; }
            else if (value == 4) { return "oc"; }
            else { return ""; }
        }
        public static string GetQuestionTypeName(int value)
        {
            if (value == 1) { return "MCQ's"; }
            else if (value == 2) { return "Smilies choice"; }
            else if (value == 3) { return "Like scale"; }
            else if (value == 4) { return "Open"; }
            else { return "Not Defined"; }
        }
    }

    public static class Operationsfor
    {
        /// <summary>
        /// Create a New Record
        /// </summary>
        public static String Quiz { get { return "q"; } }

        /// <summary>
        /// Survey
        /// </summary>
        public static String Survey { get { return "s"; } }
        
    }

    

    public static class Variables
	{
        //public ApplicationDbContext db = new ApplicationDbContext();
        //static int getage()
        //{
            
        //    var fvf = User.Identity.GetUserId();
        //    var date = DateTime.Now - db.Users.Where(p => p.Id == "jbnknkjk").FirstOrDefault().DateOfBirth;

        //    return 0;
        //}
    }
}