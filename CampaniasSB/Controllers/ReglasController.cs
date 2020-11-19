using CampaniasSB.Classes;
using CampaniasSB.Filters;
using CampaniasSB.Models;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace CampaniasSB.Controllers
{
    [Authorize]
    public class ReglasController : Controller
    {
        private readonly CampaniasContext db = new CampaniasContext();

        public string modulo = "Reglas";
        public string movimiento = string.Empty;

        public class spReglas
        {
            public int ReglaId { get; set; }
            public int ArticuloId { get; set; }
            public string Material { get; set; }
            public string Regla { get; set; }
        }

        public class spReglasCaracteristicas
        {
            public int ReglaCaracteristicaId { get; set; }
            public int ReglaId { get; set; }
            public int ReglaCatalogoId { get; set; }
            public string Caracteristica { get; set; }
            public string Valor { get; set; }
            public string Regla { get; set; }
            public bool Seleccionado { get; set; }
            public bool IsTrue { get; set; }
            public bool IsFalse { get; set; }
        }


        public class spReglaMateriales
        {
            public int ReglaMaterialId { get; set; }
            public int ReglaId { get; set; }
            public int ArticuloKFCId { get; set; }
            public string Descripcion { get; set; }
        }

        // GET: Reglas
        [AuthorizeUser(idOperacion: 5)]
        public ActionResult Index()
        {
            Session["iconoTitulo"] = "fas fa-file-contract";
            Session["homeB"] = string.Empty;
            Session["equityB"] = "active";
            Session["franquiciasB"] = string.Empty;
            Session["reglasCatalogoB"] = string.Empty;
            Session["stockB"] = string.Empty;
            Session["materialesB"] = string.Empty;

            return View();
        }
        public ActionResult GetDataEquity()
        {
            var reglasList = db.Database.SqlQuery<spReglas>("spGetReglasAll").ToList();

            return Json(new { data = reglasList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataFranquicias()
        {
            var reglasList = db.Database.SqlQuery<spReglas>("spGetReglasFranquicias").ToList();

            return Json(new { data = reglasList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataCatalogo()
        {
            var reglasList = db.Database.SqlQuery<ReglaCatalogo>("spGetReglasCatalogo").ToList().OrderBy(x => x.ReglaCatalogoId);

            return Json(new { data = reglasList }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                ViewBag.ArticuloKFCId = new SelectList(CombosHelper.GetMateriales("", true), "ArticuloKFCId", "Descripcion");

                return PartialView(new Regla());
            }
            else
            {
                var articuloId = db.Reglas.Where(x => x.ReglaId == id).FirstOrDefault().ArticuloId;

                ViewBag.ArticuloKFCId = new SelectList(CombosHelper.GetMateriales("", true), "ArticuloKFCId", "Descripcion", articuloId);
                return PartialView(db.Reglas.Where(x => x.ReglaId == id).FirstOrDefault());
            }
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult AddOrEdit(Regla regla)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            if (regla.ReglaId == 0)
            {
                db.Reglas.Add(regla);
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    var reglaId = regla.ReglaId;
                    var cat = db.Articulos.Where(x => x.ArticuloId == regla.ArticuloId).FirstOrDefault().SencilloMultiple;

                    MovementsHelper.AgregarReglasCaracteristicas(cat);

                    movimiento = "Agregar Regla " + regla.ReglaId + " " + regla.NombreRegla;
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
                db.Entry(regla).State = EntityState.Modified;
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    var reglaId = regla.ReglaId;
                    var cat = db.Articulos.Where(x => x.ArticuloId == regla.ArticuloId).FirstOrDefault().SencilloMultiple;

                    MovementsHelper.AgregarReglasCaracteristicas(cat);

                    movimiento = "Actualizar Regla " + regla.ReglaId + " " + regla.NombreRegla;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    return Json(new { success = true, message = "REGLA ACTUALIZADA" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult AddOrEditCat(int id = 0)
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
        public ActionResult AddOrEditCat(ReglaCatalogo reglaCatalogo, FormCollection fcol)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            string[] fc = fcol.GetValues("FC");
            string[] fs = fcol.GetValues("FS");
            string[] il = fcol.GetValues("IL");
            string[] sb = fcol.GetValues("SB");

            var fcTipo = 0;
            var fsTipo = 0;
            var ilTipo = 0;
            var sbTipo = 0;

            if (fc != null)
            {
                fcTipo = 1;
            }

            if (fs != null)
            {
                fsTipo = 2;
            }
            if (il != null)
            {
                ilTipo = 3;
            }
            if (sb != null)
            {
                sbTipo = 4;
            }

            if (reglaCatalogo.ReglaCatalogoId == 0)
            {
                if (string.IsNullOrEmpty(reglaCatalogo.Valor))
                {
                    reglaCatalogo.SiNo = true;
                    reglaCatalogo.Valor = "SI / NO";
                }
                else
                {
                    reglaCatalogo.SiNo = false;
                }

                db.ReglasCatalogo.Add(reglaCatalogo);
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    var cat = reglaCatalogo.Categoria;

                    var reglaIdTienda = reglaCatalogo.ReglaCatalogoId;

                    MovementsHelper.AgregarReglasCaracteristicas(cat);

                    MovementsHelper.AgregarTiendasCaracteristicas(reglaIdTienda, cat, fcTipo, fsTipo, ilTipo, sbTipo);

                    movimiento = "Agregar Característica " + reglaIdTienda + " " + reglaCatalogo.Nombre + " / " + cat;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    return Json(new { success = true, message = "CARACTERÍSTICA AGREGADA" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                if (string.IsNullOrEmpty(reglaCatalogo.Valor))
                {
                    reglaCatalogo.SiNo = true;
                    reglaCatalogo.Valor = "SI / NO";
                }
                else
                {
                    reglaCatalogo.SiNo = false;
                }

                db.Entry(reglaCatalogo).State = EntityState.Modified;
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    movimiento = "Actualizar Característica " + reglaCatalogo.ReglaCatalogoId + " " + reglaCatalogo.Nombre + " / " + reglaCatalogo.Categoria;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    return Json(new { success = true, message = "CARACTERÍSTICA ACTUALIZADA" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeUser(idOperacion: 2)]
        [HttpGet]
        public ActionResult Caracteristicas(int? id)
        {
            var reglasList = db.Database.SqlQuery<spReglasCaracteristicas>("spReglasCaracteristicas @ReglaId",
                new SqlParameter("@ReglaId", id)).OrderBy(x => x.ReglaCatalogoId).ToList();

            if (reglasList.Count == 0)
            {
                var artId = db.Reglas.Where(x => x.ReglaId == id).FirstOrDefault().ArticuloId;
                var cat = db.Articulos.Where(x => x.ArticuloId == artId).FirstOrDefault().SencilloMultiple;
                int reglaId = (int)id;

                MovementsHelper.AgregarReglasCaracteristicas(cat);
            }

            if (reglasList == null)
            {
                return HttpNotFound();
            }

            ViewBag.Regla = db.Reglas.Where(t => t.ReglaId == id).FirstOrDefault().NombreRegla;

            return PartialView(reglasList);
        }

        [AuthorizeUser(idOperacion: 2)]
        [HttpPost]
        public ActionResult Caracteristicas(FormCollection fc)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            string[] reglaCaractersiticaId = fc.GetValues("ReglaCaractersiticaId");
            //string[] seleccionado = fc.GetValues("Seleccionado");
            string[] isTrue = fc.GetValues("IsTrue");
            string[] isFalse = fc.GetValues("IsFalse");

            var selec = false;

            for (var i = 0; i < reglaCaractersiticaId.Length; i++)
            {
                ReglaCaracteristica reglaCaracteristica = db.ReglasCaracteristicas.Find(Convert.ToInt32(reglaCaractersiticaId[i]));

                var reglaId = reglaCaracteristica.ReglaId;

                if (isTrue == null)
                {
                    selec = false;

                    reglaCaracteristica.IsTrue = selec;

                    db.Entry(reglaCaracteristica).State = EntityState.Modified;

                    db.SaveChanges();
                }
                else
                {
                    for (var j = 0; j < isTrue.Length; j++)
                    {
                        if (reglaCaractersiticaId[i] == isTrue[j])
                        {
                            selec = true;

                            reglaCaracteristica.IsTrue = selec;

                            db.Entry(reglaCaracteristica).State = EntityState.Modified;
                            db.SaveChanges();

                            break;
                        }
                        else
                        {
                            selec = false;

                            reglaCaracteristica.IsTrue = selec;

                            db.Entry(reglaCaracteristica).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    if (!selec)
                    {
                        selec = false;

                        reglaCaracteristica.IsTrue = selec;

                        db.Entry(reglaCaracteristica).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                if (isFalse == null)
                {
                    selec = false;

                    reglaCaracteristica.IsFalse = selec;

                    db.Entry(reglaCaracteristica).State = EntityState.Modified;

                    db.SaveChanges();
                }
                else
                {
                    for (var j = 0; j < isFalse.Length; j++)
                    {
                        if (reglaCaractersiticaId[i] == isFalse[j])
                        {
                            selec = true;

                            reglaCaracteristica.IsFalse = selec;

                            db.Entry(reglaCaracteristica).State = EntityState.Modified;
                            db.SaveChanges();

                            break;
                        }
                        else
                        {
                            selec = false;

                            reglaCaracteristica.IsFalse = selec;

                            db.Entry(reglaCaracteristica).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    if (!selec)
                    {
                        selec = false;

                        reglaCaracteristica.IsFalse = selec;

                        db.Entry(reglaCaracteristica).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }

            movimiento = "Asignar Características";
            MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

            return Json(new { success = true, message = "CARACTERÍSTICAS ASIGNADAS" }, JsonRequestBehavior.AllowGet);

        }

        [AuthorizeUser(idOperacion: 3)]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            Regla regla = db.Reglas.Where(x => x.ReglaId == id).FirstOrDefault();
            db.Reglas.Remove(regla);
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                db.Database.ExecuteSqlCommand("spEliminarReglasCaracteristicas @ReglaId",
                new SqlParameter("@ReglaId", id));

                movimiento = "Eliminar Regla " + regla.ReglaId + " " + regla.NombreRegla;
                MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                return Json(new { success = true, message = "REGLA ELIMINADA" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeUser(idOperacion: 3)]
        [HttpPost]
        public ActionResult DeleteCat(int id)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            var tiendaCaracteristica = db.TiendaCaracteristicas.Where(x => x.ReglaCatalogoId == id).ToList();

            db.TiendaCaracteristicas.RemoveRange(tiendaCaracteristica);
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                ReglaCatalogo reglaCatalogo = db.ReglasCatalogo.Where(x => x.ReglaCatalogoId == id).FirstOrDefault();
                db.ReglasCatalogo.Remove(reglaCatalogo);
                var response2 = DBHelper.SaveChanges(db);
                if (response2.Succeeded)
                {
                    var reglaCaracteristica = db.ReglasCaracteristicas.Where(x => x.ReglaCatalogoId == id).ToList();
                    db.ReglasCaracteristicas.RemoveRange(reglaCaracteristica);
                    DBHelper.SaveChanges(db);

                    movimiento = "Eliminar Característica " + reglaCatalogo.ReglaCatalogoId + " " + reglaCatalogo.Nombre + " / " + reglaCatalogo.Categoria;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    return Json(new { success = true, message = "CARACTERÍSTICA ELIMINADA" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response2.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}