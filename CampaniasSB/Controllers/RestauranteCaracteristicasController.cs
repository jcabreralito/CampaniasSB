using CampaniasSB.Classes;
using CampaniasSB.Filters;
using CampaniasSB.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CampaniasSB.Controllers
{
    [Authorize]
    public class RestauranteCaracteristicasController : Controller
    {
        private readonly CampaniasContext db = new CampaniasContext();

        // GET: RestauranteCaracteristicas
        public ActionResult Index()
        {
            Session["iconoTitulo"] = "fas fa-clipboard-check";
            Session["homeB"] = string.Empty;
            Session["equityB"] = string.Empty;
            Session["franquiciasB"] = string.Empty;
            Session["stockB"] = string.Empty;
            Session["restaurantesB"] = string.Empty;
            Session["materialesB"] = string.Empty;
            Session["campañasB"] = string.Empty;
            Session["caracteristicasB"] = "active";

            return View();
        }
        public ActionResult GetData()
        {
            var caracteristicasList = db.Database.SqlQuery<TiendaConfiguracion>("spGetConfiguracionesTiendas").ToList();
            //var regionList = db.Regions.ToList();

            return Json(new { data = caracteristicasList }, JsonRequestBehavior.AllowGet);
        }


        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                ViewBag.EquityFranquicia = new SelectList(CombosHelper.GetTipoCampañasAll(true), "Nombre", "Nombre");
                ViewBag.TipoConfiguracion = new SelectList(CombosHelper.GetTipoConfiguracion(true), "Nombre", "Nombre");
                return PartialView(new TiendaConfiguracion());
            }
            else
            {
                var tipo = db.TiendaConfiguraciones.Where(x => x.TiendaConfiguracionId == id).FirstOrDefault().EquityFranquicia;
                var tipoC = db.TiendaConfiguraciones.Where(x => x.TiendaConfiguracionId == id).FirstOrDefault().TipoConfiguracion;
                ViewBag.EquityFranquicia = new SelectList(CombosHelper.GetTipoCampañasAll(true), "Nombre", "Nombre", tipo);
                ViewBag.TipoConfiguracion = new SelectList(CombosHelper.GetTipoConfiguracion(true), "Nombre", "Nombre", tipoC);

                return PartialView(db.TiendaConfiguraciones.Where(x => x.TiendaConfiguracionId == id).FirstOrDefault());
            }
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult AddOrEdit(TiendaConfiguracion tiendaConfiguracion)
        {
            if (tiendaConfiguracion.TiendaConfiguracionId == 0)
            {
                db.TiendaConfiguraciones.Add(tiendaConfiguracion);
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return Json(new { success = true, message = "CARACTERÍSTICA AGREGADA" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                db.Entry(tiendaConfiguracion).State = EntityState.Modified;
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return Json(new { success = true, message = "CARACTERÍSTICA ACTUALIZADA" }, JsonRequestBehavior.AllowGet);
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
            TiendaConfiguracion tiendaConfiguracion = db.TiendaConfiguraciones.Where(x => x.TiendaConfiguracionId == id).FirstOrDefault();

            tiendaConfiguracion.Eliminado = true;

            db.Entry(tiendaConfiguracion).State = EntityState.Modified;
            var response = DBHelper.SaveChanges(db);

            if (response.Succeeded)
            {
                return Json(new { success = true, message = "CARACTERÍSTICA ELIMINADA" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}