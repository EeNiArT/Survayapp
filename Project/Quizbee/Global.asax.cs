using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Quizbee
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

		public override string GetVaryByCustomString(HttpContext context, string arg)
		{
			if (arg.Equals("User", StringComparison.InvariantCultureIgnoreCase))
			{
				var user = context.User.Identity;
				return string.Format("{0}@{1}", user.Name, user.GetUserId());
			}

			return base.GetVaryByCustomString(context, arg);
		}
	}
}
