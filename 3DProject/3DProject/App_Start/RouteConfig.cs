using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace _3DProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("activation", "account/activate/{code}", new { controller = "Account", action = "Activate", code = UrlParameter.Optional });

            routes.MapRoute("userprofile", "{username}", new { controller = "Home", action = "ProfileUser", code = UrlParameter.Optional });

            routes.MapRoute("productdetail", "Products/{productname}", new { controller = "Home", action = "ProductDetail", code = UrlParameter.Optional });

            routes.MapRoute("userdetail", "User/{username}", new { controller = "Home", action = "UserDetail", code = UrlParameter.Optional });

            routes.MapRoute("resetpassword", "account/resetpassword/{code}", new { controller = "Account", action = "ResetPassword", code = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
