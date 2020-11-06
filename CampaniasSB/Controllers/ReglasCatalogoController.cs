using CampaniasSB.Classes;
using CampaniasSB.Filters;
using CampaniasSB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampaniasSB.Controllers
{
    [Authorize]
    public class ReglasCatalogoController : Controller
    {
        private readonly CampaniasContext db = new CampaniasContext();

        public string modulo = "Regla Catálogo";
        public string movimiento = string.Empty;

        // GET: ReglasCatalogo
        public ActionResult Index()
        {
            Session["iconoTitulo"] = "fas fa-file-contract";
            Session["homeB"] = string.Empty;
            Session["rolesB"] = string.Empty;
            Session["compañiasB"] = string.Empty;
            Session["usuariosB"] = string.Empty;
            Session["regionesB"] = string.Empty;
            Session["ciudadesB"] = string.Empty;
            Session["restaurantesB"] = string.Empty;
            Session["familiasB"] = string.Empty;
            Session["materialesB"] = string.Empty;
            Session["campañasB"] = string.Empty;
            Session["reglasCB"] = "active";
            Session["bitacoraB"] = string.Empty;

            return View();
        }
        public ActionResult GetData()
        {
            var reglasCatalogoList = db.Database.SqlQuery<ReglaCatalogo>("spGetReglasCatalogo").ToList();

            return Json(new { data = reglasCatalogoList }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult AddOrEdit(int id)
        {
            if (id == 0)
            {
                return PartialView(new ReglaCatalogo());
            }
            else
            {
                return PartialView(db.ReglasCatalogo.Where(x => x.ReglaCatalogoId == id).FirstOrDefault());
            }
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult AddOrEdit(ReglaCatalogo reglaCatalogo)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            if (reglaCatalogo.ReglaCatalogoId == 0)
            {
                db.ReglasCatalogo.Add(reglaCatalogo);
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    movimiento = "Agregar Regla " + reglaCatalogo.ReglaCatalogoId + " " + reglaCatalogo.Nombre + " / " + reglaCatalogo.Categoria;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    return Json(new { success = true, message = "REGLA AGREGADA" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                db.Entry(reglaCatalogo).State = EntityState.Modified;
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    movimiento = "Actualizar Regla " + reglaCatalogo.ReglaCatalogoId + " " + reglaCatalogo.Nombre + " / " + reglaCatalogo.Categoria;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    return Json(new { success = true, message = "REGLA ACTUALIZADA" }, JsonRequestBehavior.AllowGet);
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

            ReglaCatalogo reglaCatalogo = db.ReglasCatalogo.Where(x => x.ReglaCatalogoId == id).FirstOrDefault();
            db.ReglasCatalogo.Remove(reglaCatalogo);
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                movimiento = "Eliminar Regla " + reglaCatalogo.ReglaCatalogoId + " " + reglaCatalogo.Nombre + " / " + reglaCatalogo.Categoria;
                MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                return Json(new { success = true, message = "REGLA ELIMINADA" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}