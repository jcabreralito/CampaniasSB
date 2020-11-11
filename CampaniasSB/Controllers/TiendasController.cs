using CampaniasSB.Classes;
using CampaniasSB.Filters;
using CampaniasSB.Models;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CampaniasSB.Controllers
{
    [Authorize]
    public class TiendasController : Controller
    {
        private readonly CampaniasContext db = new CampaniasContext();

        public string modulo = "Tiendas";
        public string movimiento = string.Empty;

        public class spTiendasArticulos
        {
            public long TiendaArticuloId { get; set; }
            public int ArticuloId { get; set; }
            public int TiendaId { get; set; }
            public bool Seleccionado { get; set; }
            public int CantidadDefault { get; set; }
            public string NombreTienda { get; set; }
            public string Material { get; set; }
        }

        public class spTiendasCaracteristicas
        {
            public int TiendaId { get; set; }
            public string Clasificacion { get; set; }
            public string CCoFranquicia { get; set; }
            public string Restaurante { get; set; }
            public string Region { get; set; }
            public string Ciudad { get; set; }
            public string Direccion { get; set; }
            public string Observaciones { get; set; }
            public bool Activo { get; set; }
            public string Nombre { get; set; }
            public string Categoria { get; set; }
            public string Valor { get; set; }
            public int TipoConfiguracionId { get; set; }
            public int ReglaCatalogoId { get; set; }
        }

        public class spTiendasCaracteristicasFQ
        {
            public int TiendaId { get; set; }
            public string Clasificacion { get; set; }
            public string CCoFranquicia { get; set; }
            public string Restaurante { get; set; }
            public string Region { get; set; }
            public string Ciudad { get; set; }
            public string Direccion { get; set; }
            public string Observaciones { get; set; }
            public bool Activo { get; set; }
            public string Nombre { get; set; }
            public string Categoria { get; set; }
            public string Valor { get; set; }
            public int TipoConfiguracionId { get; set; }
            public int ReglaCatalogoId { get; set; }
        }

        public class spTiendasCaracteristicasSK
        {
            public int TiendaId { get; set; }
            public string Clasificacion { get; set; }
            public string CCoFranquicia { get; set; }
            public string Restaurante { get; set; }
            public string Region { get; set; }
            public string Ciudad { get; set; }
            public string Direccion { get; set; }
            public string Observaciones { get; set; }
            public bool Activo { get; set; }
            public string Nombre { get; set; }
            public string Categoria { get; set; }
            public string Valor { get; set; }
            public int TipoConfiguracionId { get; set; }
            public int ReglaCatalogoId { get; set; }
        }

        public class spRestaurantesCaracteristicas
        {
            public int TiendaCaracteristicaId { get; set; }
            public int TiendaId { get; set; }
            public string Nombre { get; set; }
            public string Valor { get; set; }
            public int TipoConfiguracionId { get; set; }
            public string CCoFranquicia { get; set; }
            public string Restaurante { get; set; }
            public int ReglaCatalogoId { get; set; }
        }

        public class spTiendas
        {
            public int TiendaId { get; set; }
            public string Esquema { get; set; }
            public string NoTienda { get; set; }
            public string NombreTienda { get; set; }
            public string Direccion { get; set; }
            public string BIS { get; set; }
            public string Idioma { get; set; }
            public string Observaciones { get; set; }
            public bool Activo { get; set; }
            public bool Eliminado { get; set; }
            public string Region { get; set; }
            public string Ciudad { get; set; }

        }

        // GET: Restaurantes
        [AuthorizeUser(idOperacion: 5)]
        public async Task<ActionResult> Index()
        {
            Session["iconoTitulo"] = "fas fa-store";
            Session["titulo"] = "EQUITY";
            Session["homeB"] = string.Empty;
            Session["equityB"] = "active";
            Session["franquiciasB"] = string.Empty;
            Session["stockB"] = string.Empty;
            Session["restaurantesB"] = string.Empty;
            Session["materialesB"] = string.Empty;
            Session["campañasB"] = string.Empty;
            Session["caracteristicasB"] = string.Empty;

            return View();

        }

        public ActionResult GetData()
        {
            var equityList = db.Database.SqlQuery<spTiendas>("spGetTiendas").ToList();

            return Json(new { data = equityList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMateriales()
        {
            var id = (int)Session["TiendaIdMaterial"];

            var materialesList = db.Database.SqlQuery<spTiendasArticulos>("spGetTiendaArticulos @TiendaId",
                                new SqlParameter("@TiendaID", id)).ToList();

            return Json(new { data = materialesList }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                ViewBag.RegionId = new SelectList(CombosHelper.GetRegiones(true), "RegionId", "Nombre");
                ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(true), "CiudadId", "Nombre");

                return PartialView(new Tienda());
            }
            else
            {

                var regionId = db.Tiendas.Where(x => x.TiendaId == id).FirstOrDefault().RegionId;
                var ciudadId = db.Tiendas.Where(x => x.TiendaId == id).FirstOrDefault().CiudadId;

                ViewBag.RegionId = new SelectList(CombosHelper.GetRegiones(true), "RegionId", "Nombre", regionId);
                ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(true), "CiudadId", "Nombre", ciudadId);

                return PartialView(db.Tiendas.Where(x => x.TiendaId == id).FirstOrDefault());
            }
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult AddOrEdit(Tienda tienda)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            if (tienda.TiendaId == 0)
            {
                tienda.Activo = true;

                db.Tiendas.Add(tienda);
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {

                    var reglasList = db.Database.SqlQuery<ReglaCatalogo>("spGetReglasCatalogoCategoria").ToList();

                    foreach (var regla in reglasList)
                    {
                        var val = string.Empty;

                        if (regla.Valor == "SI / NO")
                        {
                            val = "NO";
                        }
                        else if (regla.Valor == "TIPOS")
                        {
                            val = "FC";
                        }
                        else if (regla.Valor == "NUEVO NIVEL DE PRECIO")
                        {
                            val = "ALTO";
                        }
                        else
                        {
                            val = regla.Valor;
                        }

                        db.Database.ExecuteSqlCommand(
                        "spAgregarTiendaCaracteristicas @TiendaId, @ReglaCatalogoId, @Valor",
                        new SqlParameter("@TiendaId", tienda.TiendaId),
                        new SqlParameter("@ReglaCatalogoId", regla.ReglaCatalogoId),
                        new SqlParameter("@Valor", val));
                    }


                    MovimientosRestaurantes(tienda);

                    movimiento = "Agregar Restaurante " + tienda.TiendaId + " " + tienda.NombreTienda;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    return Json(new { success = true, message = "RESTAURANTE AGREGADO" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                db.Entry(tienda).State = EntityState.Modified;
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {

                    EliminarTodo(tienda);

                    MovimientosRestaurantes(tienda);

                    movimiento = "Actualizar Restaurante " + tienda.TiendaId + " " + tienda.NombreTienda;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    return Json(new { success = true, message = "RESTAURANTE ACTUALIZADO" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        private void MovimientosRestaurantes(Tienda tienda)
        {
            if (tienda.Activo == true)
            {
                var response = MovementsHelper.AgregarTiendaArticulos(tienda.TiendaId);

                if (response.Succeeded)
                {
                    var campaña = db.Campañas.Where(x => x.Generada == "NO").ToList();
                    if (campaña.Count >= 1)
                    {
                        var campañaid = campaña.FirstOrDefault().CampañaId;
                        var tiendaId = tienda.TiendaId;

                        MovementsHelper.AgregarArticuloPorTiendas(tiendaId, campañaid);
                    }
                }
            }
            else
            {
                EliminarTodo(tienda);
            }
        }

        private void EliminarTodo(Tienda tienda)
        {
            var id = tienda.TiendaId;

            db.Database.ExecuteSqlCommand(
            "spEliminarArticulosTiendas @TiendaId",
            new SqlParameter("@TiendaId", id));

            var campañas = db.Campañas.Where(x => x.Generada == "NO").ToList();

            if (campañas != null)
            {
                foreach (var campaña in campañas)
                {
                    var campId = campaña.CampañaId;

                    db.Database.ExecuteSqlCommand(
                    "spEliminarTiendaCampanias @TiendaId, @CampañaId",
                    new SqlParameter("@TiendaId", id),
                    new SqlParameter("@CampañaId", campId));
                }
            }
        }

        [AuthorizeUser(idOperacion: 2)]
        [HttpGet]
        public ActionResult Materiales(long? id, string cat)
        {

            var materialesList = db.Database.SqlQuery<spTiendasArticulos>("spGetTiendaArticulos @TiendaId",
                new SqlParameter("@TiendaID", id)).ToList();

            if (materialesList == null)
            {
                return HttpNotFound();
            }

            ViewBag.Campañas = db.Campañas.Where(x => x.Generada == "NO").ToList();

            ViewBag.Restaurante = db.Tiendas.Where(t => t.TiendaId == id).FirstOrDefault().NombreTienda;

            return PartialView(materialesList);
        }

        [AuthorizeUser(idOperacion: 2)]
        [HttpPost]
        public ActionResult Materiales(FormCollection fc)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            string[] campañaId = fc.GetValues("Campaña");
            string[] articuloKFCTMPId = fc.GetValues("TiendaArticuloId");
            string[] seleccionado = fc.GetValues("Seleccionado");

            var selec = false;
            var cantidad = 0;
            var campId = 0;

            if (campañaId == null)
            {
                for (var i = 0; i < articuloKFCTMPId.Length; i++)
                {
                    TiendaArticulo tiendaArticulo = db.TiendaArticulos.Find(Convert.ToInt32(articuloKFCTMPId[i]));

                    var tiendaId = tiendaArticulo.TiendaId;
                    var articuloId = tiendaArticulo.ArticuloId;

                    var campañas = db.Campañas.Where(ct => ct.Generada == "NO" && ct.CampañaId == campId).OrderBy(ct => ct.CampañaId).FirstOrDefault();

                    if (campañas == null)
                    {

                        if (seleccionado == null)
                        {
                            selec = false;
                            cantidad = 0;

                            tiendaArticulo.Seleccionado = selec;

                            db.Entry(tiendaArticulo).State = EntityState.Modified;

                            db.SaveChanges();
                        }
                        else
                        {
                            for (var j = 0; j < seleccionado.Length; j++)
                            {
                                if (articuloKFCTMPId[i] == seleccionado[j])
                                {
                                    selec = true;

                                    tiendaArticulo.Seleccionado = selec;

                                    db.Entry(tiendaArticulo).State = EntityState.Modified;
                                    db.SaveChanges();

                                    break;
                                }
                            }
                            if (!selec)
                            {
                                selec = false;
                                cantidad = 0;

                                tiendaArticulo.Seleccionado = selec;

                                db.Entry(tiendaArticulo).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        campId = campañas.CampañaId;
                        CampañaArticulo campañaArticulo = db.CampañaArticulos.Where(ta => ta.TiendaId == tiendaId && ta.ArticuloId == articuloId && ta.CampañaId == campId).FirstOrDefault();

                        selec = false;
                        cantidad = 0;
                        if (campañaArticulo == null)
                        {
                            var codigo = 0;

                            db.Database.ExecuteSqlCommand(
                            "spAgregarMaterialCAmpanias @ArticuloKFCId, @TiendaId, @CampañaId, @Habilitado, @Cantidad, @Codigo",
                            new SqlParameter("@ArticuloKFCId", articuloId),
                            new SqlParameter("@TiendaId", tiendaId),
                            new SqlParameter("@CampañaId", campId),
                            new SqlParameter("@Habilitado", false),
                            new SqlParameter("@Cantidad", cantidad),
                            new SqlParameter("@Codigo", codigo));

                        }
                        else
                        {
                            if (seleccionado == null)
                            {
                                selec = false;
                                cantidad = 0;

                                tiendaArticulo.Seleccionado = selec;

                                db.Entry(tiendaArticulo).State = EntityState.Modified;

                                campañaArticulo.Habilitado = selec;
                                campañaArticulo.Cantidad = cantidad;

                                db.Entry(campañaArticulo).State = EntityState.Modified;

                                db.SaveChanges();
                            }
                            else
                            {
                                for (var j = 0; j < seleccionado.Length; j++)
                                {
                                    if (articuloKFCTMPId[i] == seleccionado[j])
                                    {
                                        selec = true;

                                        tiendaArticulo.Seleccionado = selec;

                                        db.Entry(tiendaArticulo).State = EntityState.Modified;

                                        var articuloCantidadDefault = db.Articulos.Where(a => a.ArticuloId == campañaArticulo.ArticuloId).FirstOrDefault().CantidadDefault;

                                        cantidad = articuloCantidadDefault;
                                        campañaArticulo.Cantidad = cantidad;
                                        campañaArticulo.Habilitado = selec;

                                        db.Entry(campañaArticulo).State = EntityState.Modified;
                                        db.SaveChanges();

                                        break;
                                    }
                                }
                                if (!selec)
                                {
                                    selec = false;
                                    cantidad = 0;

                                    tiendaArticulo.Seleccionado = selec;

                                    db.Entry(tiendaArticulo).State = EntityState.Modified;

                                    campañaArticulo.Habilitado = selec;
                                    campañaArticulo.Cantidad = cantidad;

                                    db.Entry(campañaArticulo).State = EntityState.Modified;

                                    db.SaveChanges();
                                }
                            }
                        }
                    }

                }

            }
            else
            {
                for (int c = 0; c < campañaId.Length; c++)
                {
                    Campaña campañaArtId = db.Campañas.Find(Convert.ToInt32(campañaId[c]));
                    campId = campañaArtId.CampañaId;

                    for (var i = 0; i < articuloKFCTMPId.Length; i++)
                    {
                        TiendaArticulo tiendaArticulo = db.TiendaArticulos.Find(Convert.ToInt32(articuloKFCTMPId[i]));

                        var tiendaId = tiendaArticulo.TiendaId;
                        var articuloId = tiendaArticulo.ArticuloId;

                        var campañas = db.Campañas.Where(ct => ct.Generada == "NO" && ct.CampañaId == campId).OrderBy(ct => ct.CampañaId).FirstOrDefault();

                        if (campañas == null)
                        {

                            if (seleccionado == null)
                            {
                                selec = false;
                                cantidad = 0;

                                tiendaArticulo.Seleccionado = selec;

                                db.Entry(tiendaArticulo).State = EntityState.Modified;

                                db.SaveChanges();
                            }
                            else
                            {
                                for (var j = 0; j < seleccionado.Length; j++)
                                {
                                    if (articuloKFCTMPId[i] == seleccionado[j])
                                    {
                                        selec = true;

                                        tiendaArticulo.Seleccionado = selec;

                                        db.Entry(tiendaArticulo).State = EntityState.Modified;
                                        db.SaveChanges();

                                        break;
                                    }
                                }
                                if (!selec)
                                {
                                    selec = false;
                                    cantidad = 0;

                                    tiendaArticulo.Seleccionado = selec;

                                    db.Entry(tiendaArticulo).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            campId = campañas.CampañaId;
                            CampañaArticulo campañaArticulo = db.CampañaArticulos.Where(ta => ta.TiendaId == tiendaId && ta.ArticuloId == articuloId && ta.CampañaId == campId).FirstOrDefault();

                            selec = false;
                            cantidad = 0;
                            if (campañaArticulo == null)
                            {
                                var codigo = 0;

                                db.Database.ExecuteSqlCommand(
                                "spAgregarMaterialCAmpanias @ArticuloKFCId, @TiendaId, @CampañaId, @Habilitado, @Cantidad, @Codigo",
                                new SqlParameter("@ArticuloKFCId", articuloId),
                                new SqlParameter("@TiendaId", tiendaId),
                                new SqlParameter("@CampañaId", campId),
                                new SqlParameter("@Habilitado", false),
                                new SqlParameter("@Cantidad", cantidad),
                                new SqlParameter("@Codigo", codigo));

                            }
                            else
                            {
                                if (seleccionado == null)
                                {
                                    selec = false;
                                    cantidad = 0;

                                    tiendaArticulo.Seleccionado = selec;

                                    db.Entry(tiendaArticulo).State = EntityState.Modified;

                                    campañaArticulo.Habilitado = selec;
                                    campañaArticulo.Cantidad = cantidad;

                                    db.Entry(campañaArticulo).State = EntityState.Modified;

                                    db.SaveChanges();
                                }
                                else
                                {
                                    for (var j = 0; j < seleccionado.Length; j++)
                                    {
                                        if (articuloKFCTMPId[i] == seleccionado[j])
                                        {
                                            selec = true;

                                            tiendaArticulo.Seleccionado = selec;

                                            db.Entry(tiendaArticulo).State = EntityState.Modified;

                                            var articuloCantidadDefault = db.Articulos.Where(a => a.ArticuloId == campañaArticulo.ArticuloId).FirstOrDefault().CantidadDefault;

                                            cantidad = articuloCantidadDefault;
                                            campañaArticulo.Cantidad = cantidad;
                                            campañaArticulo.Habilitado = selec;

                                            db.Entry(campañaArticulo).State = EntityState.Modified;
                                            db.SaveChanges();

                                            break;
                                        }
                                    }
                                    if (!selec)
                                    {
                                        selec = false;
                                        cantidad = 0;

                                        tiendaArticulo.Seleccionado = selec;

                                        db.Entry(tiendaArticulo).State = EntityState.Modified;

                                        campañaArticulo.Habilitado = selec;
                                        campañaArticulo.Cantidad = cantidad;

                                        db.Entry(campañaArticulo).State = EntityState.Modified;

                                        db.SaveChanges();
                                    }
                                }
                            }
                        }

                    }
                }
            }
            TiendaArticulo tienda = db.TiendaArticulos.Find(Convert.ToInt32(articuloKFCTMPId[0]));

            movimiento = "Asignar Materiales / " + tienda.TiendaId;
            MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

            return Json(new { success = true, message = "MATERIALES ASIGNADOS" }, JsonRequestBehavior.AllowGet);

        }

        [AuthorizeUser(idOperacion: 3)]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;


            Tienda tienda = db.Tiendas.Where(x => x.TiendaId == id).FirstOrDefault();
            tienda.Eliminado = true;
            db.Entry(tienda).State = EntityState.Modified;
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                db.Database.ExecuteSqlCommand(
                "spEliminarCaracteristicasTiendas @TiendaId",
                new SqlParameter("@TiendaId", id));

                db.Database.ExecuteSqlCommand(
                "spEliminarArticulosTiendas @TiendaId",
                new SqlParameter("@TiendaId", id));

                var campañas = db.Campañas.Where(x => x.Generada == "NO").ToList();

                if (campañas != null)
                {
                    foreach (var campaña in campañas)
                    {
                        var campId = campaña.CampañaId;

                        db.Database.ExecuteSqlCommand(
                        "spEliminarTiendaCampanias @TiendaId, @CampañaId",
                        new SqlParameter("@TiendaId", id),
                        new SqlParameter("@CampañaId", campId));
                    }
                }

                movimiento = "Eliminar Restaurante " + tienda.TiendaId + " " + tienda.NombreTienda;
                MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                return Json(new { success = true, message = "RESTAURANTE ELIMINADO" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}