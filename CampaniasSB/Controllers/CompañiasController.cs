using CampaniasSB.Classes;
using CampaniasSB.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CampaniasSB.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class CompañiasController : Controller
    {
        private readonly CampaniasContext db = new CampaniasContext();

        // GET: Compañias
        public ActionResult Index()
        {
            Session["iconoTitulo"] = "fas fa-building";
            Session["homeB"] = string.Empty;
            Session["rolesB"] = string.Empty;
            Session["compañiasB"] = "active";
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
            var compañiaList = db.Compañias.ToList();

            return Json(new { data = compañiaList }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return PartialView(new Compañia());
            }
            else
            {
                return PartialView(db.Compañias.Where(x => x.CompañiaId == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Compañia compañia)
        {

            if (compañia.CompañiaId == 0)
            {
                db.Compañias.Add(compañia);
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                        CargarImagen(compañia);

                    //return View("Index");
                    return Json(new { success = true, message = "COMPAÑIA AGREGADA" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                db.Entry(compañia).State = EntityState.Modified;
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    CargarImagen(compañia);

                    //return View("Index");
                    return Json(new { success = true, message = "COMPAÑIA ACTUALIZADA" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        private void CargarImagen(Compañia compañia)
        {
            if (compañia.LogoFile != null)
            {
                var folder = "~/Content/images/Logos";
                var local = "/";
                var file = string.Format("{0}{1}.jpg", compañia.CompañiaId, compañia.Nombre);
                var responseLogo = FilesHelper.UploadPhoto(compañia.LogoFile, folder, file);
                if (responseLogo)
                {
                    var pic = string.Format("{0}/{1}", local, file);
                    compañia.Logo = pic;
                    db.Entry(compañia).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Compañia compañia = db.Compañias.Where(x => x.CompañiaId == id).FirstOrDefault();
            db.Compañias.Remove(compañia);
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                return Json(new { success = true, message = "COMPAÑIA ELIMINADA" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}