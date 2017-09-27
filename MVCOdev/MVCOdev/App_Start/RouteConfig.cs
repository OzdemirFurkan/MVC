using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCOdev
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("urunler", "urunler", new { controller = "Product", action = "List" });

            //{} içerisinde tanımlananlar değişkenlik gösteren yapılar iken kategori ve ürünler gibi yazılanlar static tanımlıdır.
            routes.MapRoute("kategori-detay", "kategori/{nameSlug}", new { controller = "Category", action = "Show", nameSlug = UrlParameter.Optional });


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Anasayfa", action = "index", id = UrlParameter.Optional }
            );
        }
    }
}
