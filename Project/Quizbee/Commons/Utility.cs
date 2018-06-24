using Quizbee.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizbee.Commons
{
    public class Utility
    {


        public static string UserRoleId
        {
            set
            {
                HttpContext.Current.Session["UserRole"] = value;
            }
            get
            {
                if (HttpContext.Current.Session["UserRole"] != null)
                {
                    HttpContext.Current.Session["UserRole"].ToString();
                }
                return "0";
            }

        }

        public static int getageofuser()
        {
            

            return 0;
        }
    }
}