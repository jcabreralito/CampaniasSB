using CampaniasSB.Classes;
using CampaniasSB.Filters;
using CampaniasSB.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CampaniasSB.Controllers
{
    [Authorize]
    public class RegionesController : Controller
    {
        private readonly CampaniasContext db = new CampaniasContext();

        public string modulo = "Regiones";
        public string movimiento = string.Empty;

        // GET: Regiones
        [AuthorizeUser(idOperacion: 5)]
        public ActionResult Index()
        {
            Session["iconoTitulo"] = "fas fa-map-marked";
            Session["homeB"] = string.Empty;
            Session["rolesB"] = string.Empty;
            Session["compañiasB"] = string.Empty;
            Session["usuariosB"] = string.Empty;
            Session["regionesB"] = "active";
            Session["ciudadesB"] = string.Empty;
            Session["restaurantesB"] = string.Empty;
            Session["familiasB"] = string.Empty;
            Session["materialesB"] = string.Empty;
            Session["campañasB"] = string.Empty;
            Session["reglasB"] = string.Empty;
            Session["bitacoraB"] = string.Empty;

            return View();
        }
        public ActionResult GetData()
        {
            var regionList = db.Database.SqlQuery<Region>("spGetRegiones").ToList();

            return Json(new { data = regionList }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return PartialView(new Region());
            }
            else
            {
                var regionList = db.Database.SqlQuery<Region>("spGetRegiones").Where(x => x.RegionId == id).FirstOrDefault();

                return PartialView(regionList);
            }
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult AddOrEdit(Region region)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;
            var nombreRegion = region.Nombre.ToUpper();

            if (region.RegionId == 0)
            {
                region.Nombre = nombreRegion;
                db.Regiones.Add(region);
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    movimiento = "Agregar Región " + region.RegionId + " " + region.Nombre;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    return Json(new { success = true, message = "REGIÓN AGREGADA" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                region.Nombre = nombreRegion;
                db.Entry(region).State = EntityState.Modified;
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    movimiento = "Actualizar Región " + region.RegionId + " " + region.Nombre.ToUpper();
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    return Json(new { success = true, message = "REGIÓN ACTUALIZADA" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeUser(idOperacion: 3)]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            Region region = db.Regiones.Where(x => x.RegionId == id).FirstOrDefault();
            db.Regiones.Remove(region);
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                movimiento = "Eliminar Región " + region.RegionId + " " + region.Nombre;
                MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                return Json(new { success = true, message = "REGIÓN ELIMINADA" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}