using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NewBlog.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Tag",
                "Tag/{tag}",
                new { controller = "Blog", action = "Tag" }
            );


            routes.MapRoute(
                "Category",
                "Category/{category}",
                new { controller = "Blog", action = "Category" }
            );

            routes.MapRoute(
                "Register",
                "Register",
                new { controller = "Account", action = "Register" }
            );

            routes.MapRoute(
                "Login",
                "Login",
                new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                "Logout",
                "Logout",
                new { controller = "Account", action = "Logout" }
            ); 

            routes.MapRoute(
                "Admin",
                "Admin/{action}",
                new { controller = "Admin", action = "{action}" }
            );

            routes.MapRoute( 
                "Action",
                "{action}",
                new { controller = "Blog", action = "Posts" }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}",
                new { controller = "Account", action = "Login" }
            );

        }
    }
}
