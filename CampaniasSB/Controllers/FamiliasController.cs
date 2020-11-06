using CampaniasSB.Classes;
using CampaniasSB.Filters;
using CampaniasSB.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CampaniasSB.Controllers
{
    [Authorize]
    public class FamiliasController : Controller
    {
        private readonly CampaniasContext db = new CampaniasContext();

        public string modulo = "Familias";
        public string movimiento = string.Empty;

        // GET: /Employee/
        [AuthorizeUser(idOperacion: 5)]
        public ActionResult Index()
        {
            Session["iconoTitulo"] = "fas fa-boxes";
            Session["homeB"] = string.Empty;
            Session["rolesB"] = string.Empty;
            Session["compañiasB"] = string.Empty;
            Session["usuariosB"] = string.Empty;
            Session["regionesB"] = string.Empty;
            Session["ciudadesB"] = string.Empty;
            Session["restaurantesB"] = string.Empty;
            Session["familiasB"] = "active";
            Session["materialesB"] = string.Empty;
            Session["campañasB"] = string.Empty;
            Session["reglasB"] = string.Empty;
            Session["bitacoraB"] = string.Empty;

            return View();
        }

        public ActionResult GetData()
        {
            var famList = db.Database.SqlQuery<Familia>("spGetFamilias").ToList();
            var familias = famList.Where(x => x.Codigo != "995" && x.Codigo != "996" && x.Codigo != "997" && x.Codigo != "998" && x.Codigo != "999").ToList();

            return Json(new { data = familias }, JsonRequestBehavior.AllowGet);
        }


        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return PartialView(new Familia());
            }
            else
            {
                return PartialView(db.Familias.Where(x => x.FamiliaId == id).FirstOrDefault());
            }
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult AddOrEdit(Familia fam)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            if (fam.FamiliaId == 0)
            {
                db.Familias.Add(fam);
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    movimiento = "Agregar Familia " + fam.FamiliaId + " " + fam.Descripcion + " / " + fam.Codigo;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    return Json(new { success = true, message = "FAMILIA AGREGADA" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                db.Entry(fam).State = EntityState.Modified;
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    movimiento = "Actualizar Familia " + fam.FamiliaId + " " + fam.Descripcion + " / " + fam.Codigo;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    return Json(new { success = true, message = "FAMILIA ACTUALIZADA" }, JsonRequestBehavior.AllowGet);
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

            Familia fam = db.Familias.Where(x => x.FamiliaId == id).FirstOrDefault();
            db.Familias.Remove(fam);
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                movimiento = "Eliminar Familia " + fam.FamiliaId + " " + fam.Descripcion + " / " + fam.Codigo;
                MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                return Json(new { success = true, message = "FAMILIA ELIMINADA" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}