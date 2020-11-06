using CampaniasSB.Classes;
using CampaniasSB.Migrations;
using CampaniasSB.Models;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace CampaniasSB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CampaniasContext, Configuration>());
            CheckRolesAndSuperUser();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private void CheckRolesAndSuperUser()
        {
            UsuariosHelper.CheckRole("SuperAdmin");
            UsuariosHelper.CheckRole("Admin");
            UsuariosHelper.CheckRole("User");
            UsuariosHelper.CheckRole("Servicio");
            UsuariosHelper.CheckRole("Cliente");
            UsuariosHelper.CheckRole("CONSULTAS");
            UsuariosHelper.CheckSuperUser();
            UsuariosHelper.CrearRoles("SuperAdmin");
            UsuariosHelper.CrearRoles("Admin");
            UsuariosHelper.CrearRoles("User");
            UsuariosHelper.CrearRoles("Servicio");
            UsuariosHelper.CrearRoles("Cliente");
            UsuariosHelper.CrearRoles("CONSULTAS");
            UsuariosHelper.CrearModulo("GENERAL");
            UsuariosHelper.CrearOperaciones("AGREGAR", 1);
            UsuariosHelper.CrearOperaciones("EDITAR", 1);
            UsuariosHelper.CrearOperaciones("ELIMINAR", 1);
            UsuariosHelper.CrearOperaciones("DETALLE", 1);
            UsuariosHelper.CrearOperaciones("CONSULTAS", 1);
            UsuariosHelper.CrearOperaciones("CERRAR", 1);
            UsuariosHelper.CrearOperaciones("CODIGOS", 1);
            UsuariosHelper.CrearRolOperaciones(1, 1);
            UsuariosHelper.CrearRolOperaciones(1, 2);
            UsuariosHelper.CrearRolOperaciones(1, 3);
            UsuariosHelper.CrearRolOperaciones(1, 4);
            UsuariosHelper.CrearRolOperaciones(1, 5);
            UsuariosHelper.CrearRolOperaciones(1, 6);
            UsuariosHelper.CrearRolOperaciones(1, 7);
            UsuariosHelper.CrearRolOperaciones(2, 1);
            UsuariosHelper.CrearRolOperaciones(2, 2);
            UsuariosHelper.CrearRolOperaciones(2, 3);
            UsuariosHelper.CrearRolOperaciones(2, 4);
            UsuariosHelper.CrearRolOperaciones(2, 5);
            UsuariosHelper.CrearRolOperaciones(2, 6);
            UsuariosHelper.CrearRolOperaciones(2, 7);
            UsuariosHelper.CrearRolOperaciones(6, 4);
            UsuariosHelper.CrearRolOperaciones(6, 5);
            UsuariosHelper.CrearRolOperaciones(3, 1);
            UsuariosHelper.CrearRolOperaciones(3, 2);
            UsuariosHelper.CrearRolOperaciones(3, 4);
            UsuariosHelper.CrearRolOperaciones(3, 5);
            UsuariosHelper.CrearCategorias("EQUITY");
            UsuariosHelper.CrearCategorias("FRANQUICIAS");
            UsuariosHelper.CrearCategorias("EQUITY / FRANQUICIAS");
            UsuariosHelper.CrearCategorias("STOCK");
        }
    }
}
