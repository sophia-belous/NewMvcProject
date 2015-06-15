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
                "Post",
                "Archive/{year}/{month}/{title}",
                new { controller = "Blog", action = "Post" }
                );


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
                "Login",
                "Login",
                new { controller = "Account", action = "Login" }
                );

            routes.MapRoute(
                "Action",
                "{action}",
                new { controller = "Blog", action = "Posts" }
                );

        }
    }
}
