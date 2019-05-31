using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ex3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "ShowLocation",
                "display/{ip}/{port}",
                new { controller = "First", action = "ShowLocation" }, 
                new {ip = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b"}
                // Check if this is a valid address ip
            );

            routes.MapRoute("ShowRoute", "display/{ip}/{port}/{rate}",
            defaults: new { controller = "First", action = "ShowRoute" });

            routes.MapRoute(
                name: "Save",
                url: "save/{ip}/{port}/{rate}/{time}/{file}",
                defaults: new { controller = "First", action = "Save" }
            );

            routes.MapRoute(
                "LoadFile",
                "display/{file}/{rate}",
                new { controller = "First", action = "LoadFile" }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "First", action = "Index", id = UrlParameter.Optional }
            );

            
        }
    }
}
