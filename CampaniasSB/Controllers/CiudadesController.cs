using CampaniasSB.Classes;
using CampaniasSB.Filters;
using CampaniasSB.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CampaniasSB.Controllers
{
    [Authorize]
    public class CiudadesController : Controller
    {
        private readonly CampaniasContext db = new CampaniasContext();

        public string modulo = "Ciudades";
        public string movimiento = string.Empty;

        public class spCiudades
        {
            public int CiudadId { get; set; }
            public string Nombre { get; set; }
            public string Region { get; set; }
        }
        // GET: Regiones
        [AuthorizeUser(idOperacion: 5)]
        public ActionResult Index()
        {
            Session["iconoTitulo"] = "fas fa-map-marker-alt";
            Session["homeB"] = string.Empty;
            Session["rolesB"] = string.Empty;
            Session["compañiasB"] = string.Empty;
            Session["usuariosB"] = string.Empty;
            Session["regionesB"] = string.Empty;
            Session["ciudadesB"] = "active";
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
            var ciudadList = db.Database.SqlQuery<spCiudades>("spGetCiudades").ToList();

            return Json(new { data = ciudadList }, JsonRequestBehavior.AllowGet);
        }


        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                ViewBag.RegionId = new SelectList(CombosHelper.GetRegiones(true), "RegionId", "Nombre");
                return PartialView(new Ciudad());
            }
            else
            {
                var regionId = db.Ciudades.Where(x => x.CiudadId == id).FirstOrDefault().RegionId;
                ViewBag.RegionId = new SelectList(CombosHelper.GetRegiones(true), "RegionId", "Nombre", regionId);
                return PartialView(db.Ciudades.Where(x => x.CiudadId == id).FirstOrDefault());
            }
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult AddOrEdit(Ciudad ciudad)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;
            var nombreCiudad = ciudad.Nombre.ToUpper();

            if (ciudad.CiudadId == 0)
            {
                ciudad.Nombre = nombreCiudad;
                db.Ciudades.Add(ciudad);
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    movimiento = "Agregar Ciudad " + ciudad.CiudadId + " " + ciudad.Nombre;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    return Json(new { success = true, message = "CIUDAD AGREGADA" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ciudad.Nombre = nombreCiudad;
                db.Entry(ciudad).State = EntityState.Modified;
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    movimiento = "Actualizar Ciudad " + ciudad.CiudadId + " " + ciudad.Nombre;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    return Json(new { success = true, message = "CIUDAD ACTUALIZADA" }, JsonRequestBehavior.AllowGet);
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

            Ciudad ciudad = db.Ciudades.Where(x => x.CiudadId == id).FirstOrDefault();
            db.Ciudades.Remove(ciudad);
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                movimiento = "Eliminar Ciudad " + ciudad.CiudadId + " " + ciudad.Nombre;
                MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                return Json(new { success = true, message = "CIUDAD ELIMINADA" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}