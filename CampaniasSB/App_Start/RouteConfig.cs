using System.Web.Mvc;
using System.Web.Routing;

namespace CampaniasSB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "IndexAdmin",
                url: "InicioAdmin",
                defaults: new { controller = "Home", action = "IndexAdmin" }
            );

            routes.MapRoute(
                name: "Index",
                url: "Inicio",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                name: "IndexSB",
                url: "Tiendas",
                defaults: new { controller = "Tiendas", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
