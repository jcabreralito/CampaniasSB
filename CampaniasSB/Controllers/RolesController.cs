using CampaniasSB.Classes;
using CampaniasSB.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CampaniasSB.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class RolesController : Controller
    {
        private static readonly ApplicationDbContext userContext = new ApplicationDbContext();
        private readonly CampaniasContext db = new CampaniasContext();

        // GET: Roles
        public ActionResult Index()
        {
            Session["iconoTitulo"] = "fas fa-user-lock";
            Session["homeB"] = string.Empty;
            Session["rolesB"] = "active";
            Session["compañiasB"] = string.Empty;
            Session["usuariosB"] = string.Empty;
            Session["regionesB"] = string.Empty;
            Session["ciudadesB"] = string.Empty;
            Session["restaurantesB"] = string.Empty;
            Session["familiasB"] = string.Empty;
            Session["materialesB"] = string.Empty;
            Session["campañasB"] = string.Empty;
            Session["reglasB"] = string.Empty;

            return View();
        }
        public ActionResult GetData()
        {
            var rolList = db.Roles.Where(x => x.Nombre != "SuperAdmin" && x.Nombre != "Admin").ToList();

            return Json(new { data = rolList }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return PartialView(new Rol());
            }
            else
            {
                var currentRol = db.Roles.Where(x => x.RolId == id).FirstOrDefault().Nombre;

                Session["CurrentRol"] = currentRol;

                return PartialView(db.Roles.Where(x => x.RolId == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Rol rol)
        {

            if (rol.RolId == 0)
            {
                db.Roles.Add(rol);
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    UsuariosHelper.CheckRole(rol.Nombre);

                    return Json(new { success = true, message = "ROL AGREGADO" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                db.Entry(rol).State = EntityState.Modified;
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    UsuariosHelper.UpdateRole(Session["CurrentRol"].ToString(), rol.Nombre);
                    return Json(new { success = true, message = "ROL ACTUALIZADO" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Rol rol = db.Roles.Where(x => x.RolId == id).FirstOrDefault();
            db.Roles.Remove(rol);
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                return Json(new { success = true, message = "ROL ELIMINADO" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}