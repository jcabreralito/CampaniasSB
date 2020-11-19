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
    public class MaterialesController : Controller
    {
        public string modulo = "Artículos";
        public string movimiento = string.Empty;

        public class MaterialesCampaña
        {
            public int ArticuloId { get; set; }
            public string Articulo { get; set; }
            public string Campaña { get; set; }
            public int CampañaId { get; set; }
            public double Cantidad { get; set; }
            public int TiendaId { get; set; }
            public bool Habilitado { get; set; }
        }

        public class TiendasCampaña
        {
            public int TiendaId { get; set; }
            public string Restaurante { get; set; }
            public string Clasificacion { get; set; }
            public string CC { get; set; }
            public string TipoTienda { get; set; }
            public string Region { get; set; }
            public string Ciudad { get; set; }
            public string Direccion { get; set; }

        }

        public class MaterialesTiendasCampaña
        {
            public int ArticuloId { get; set; }
            public string Articulo { get; set; }
            public string Campaña { get; set; }
            public int CampañaId { get; set; }
            public double Cantidad { get; set; }
            public int TiendaId { get; set; }
            public string Restaurante { get; set; }
            public string Clasificacion { get; set; }
            public string CC { get; set; }
            public string Region { get; set; }
            public string Ciudad { get; set; }
            public string Direccion { get; set; }
            public bool Habilitado { get; set; }
        }

        public class spArticulosTiendas
        {
            public long TiendaArticuloId { get; set; }
            public int ArticuloId { get; set; }
            public int TiendaId { get; set; }
            public bool Seleccionado { get; set; }
            public int CantidadDefault { get; set; }
            public string NombreTienda { get; set; }
            public string Material { get; set; }
            public string SencilloMultiple { get; set; }
            public string NoTienda { get; set; }

        }

        public class spArticulo
        {
            public int ArticuloId { get; set; }
            public string Descripcion { get; set; }
            public int CantidadDefault { get; set; }
            public string SencilloMultiple { get; set; }
            public string Observaciones { get; set; }
            public bool Eliminado { get; set; }
            public bool Activo { get; set; }
            public string Imagen { get; set; }

        }

        private readonly CampaniasContext db = new CampaniasContext();

        // GET: Materiales
        [AuthorizeUser(idOperacion: 5)]
        public ActionResult Index()
        {
            Session["iconoTitulo"] = "fas fa-barcode";
            Session["homeB"] = string.Empty;
            Session["rolesB"] = string.Empty;
            Session["compañiasB"] = string.Empty;
            Session["usuariosB"] = string.Empty;
            Session["regionesB"] = string.Empty;
            Session["ciudadesB"] = string.Empty;
            Session["restaurantesB"] = string.Empty;
            Session["materialesB"] = "active";
            Session["campañasB"] = string.Empty;
            Session["reglasB"] = string.Empty;
            Session["bitacoraB"] = string.Empty;

            return View();
        }

        public ActionResult GetData()
        {
            var articulosList = db.Database.SqlQuery<Articulo>("spGetMaterialesAll").ToList();

            return Json(new { data = articulosList }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                ViewBag.SencilloMultiple = new SelectList(CombosHelper.GetSencilloMultiple(true), "Nombre", "Nombre");
                return PartialView(new Articulo());
            }
            else
            {

                return PartialView(db.Articulos.Where(x => x.ArticuloId == id).FirstOrDefault());
            }
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult AddOrEdit(Articulo material)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;
            //var restauranteId = 0;
            if (material.ArticuloId == 0)
            {
                material.Activo = true;

                if (material.Observaciones == null)
                {
                    material.Observaciones = string.Empty;
                }

                db.Articulos.Add(material);
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    CargarImagen(material);

                    //MovementsHelper.AgregarMaterialesTiendaCampañaExiste(material.ArticuloId, restauranteId);

                    var campaña = db.Campañas.Where(x => x.Generada == "NO").FirstOrDefault();

                    if (campaña != null)
                    {
                        var campañaId = campaña.CampañaId;

                        //MovementsHelper.AgregarArticuloCampañas(material, campañaId);
                    }

                    movimiento = "Agregar Artículo " + material.ArticuloId + " " + material.Descripcion + " / " + material.SencilloMultiple;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    return Json(new { success = true, message = "ARTÍCULO AGREGADO" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                if (material.Observaciones == null)
                {
                    material.Observaciones = string.Empty;
                }

                db.Entry(material).State = EntityState.Modified;
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    CargarImagen(material);

                    var campaña = db.Campañas.Where(x => x.Generada == "NO").FirstOrDefault();

                    var id = material.ArticuloId;

                    if (material.Activo == true)
                    {

                        if (campaña != null)
                        {
                            var campañaId = campaña.CampañaId;

                            //MovementsHelper.AgregarArticuloCampañas(material, campañaId);
                        }

                    }
                    else
                    {
                        EliminarMateriales(id, campaña);
                    }

                    movimiento = "Actualizar Artículo " + material.ArticuloId + " " + material.Descripcion + " / " + material.SencilloMultiple;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    return Json(new { success = true, message = "ARTÍCULO ACTUALIZADO" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        private void EliminarMateriales(int id, Campaña campaña)
        {
            db.Database.ExecuteSqlCommand(
            "spEliminarMaterialTiendas @ArticuloKFCId",
            new SqlParameter("@ArticuloKFCId", id));

            if (campaña != null)
            {
                db.Database.ExecuteSqlCommand(
                "spEliminarMaterialCampaniasTiendas @ArticuloKFCId, @CampaniaId",
                new SqlParameter("@ArticuloKFCId", id),
                new SqlParameter("@CampaniaId", campaña.CampañaId));
            }
        }

        private void CargarImagen(Articulo articuloKFC)
        {
            if (articuloKFC.ImagenFile != null)
            {
                var folder = "~/Content/images/Productos";
                var local = "/";
                var file = string.Format("{0}.jpg", articuloKFC.Descripcion);
                var responseLogo = FilesHelper.UploadPhoto(articuloKFC.ImagenFile, folder, file);
                if (responseLogo)
                {
                    var pic = string.Format("{0}/{1}", local, file);
                    articuloKFC.Imagen = pic;
                    db.Entry(articuloKFC).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        [AuthorizeUser(idOperacion: 2)]
        [HttpGet]
        public ActionResult Restaurantes(int? id, string cat)
        {

            var restaurantesList = db.Database.SqlQuery<spArticulosTiendas>("spGetArticuloTiendas @ArticuloKFCId, @EquityFranquicia",
                new SqlParameter("@ArticuloKFCId", id),
                new SqlParameter("@EquityFranquicia", cat)).ToList();

            if (restaurantesList == null)
            {
                return HttpNotFound();
            }

            ViewBag.Campañas = db.Campañas.Where(x => x.Generada == "NO").ToList();
            ViewBag.Material = db.Articulos.Where(x => x.ArticuloId == id).FirstOrDefault().Descripcion;

            return PartialView(restaurantesList);
        }

        [AuthorizeUser(idOperacion: 2)]
        [HttpPost]
        public ActionResult Restaurantes(FormCollection fc)
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

            TiendaArticulo articulo = db.TiendaArticulos.Find(Convert.ToInt32(articuloKFCTMPId[0]));

            movimiento = "Asignar Restaurantes / " + articulo.ArticuloId;
            MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

            return Json(new { success = true, message = "RESTAURANTES ASIGNADOS" }, JsonRequestBehavior.AllowGet);

            //return RedirectToAction("Index");

        }

        // GET: Tiendas/Edit/5
        [AuthorizeUser(idOperacion: 2)]
        public ActionResult Cantidades(int? id, int cam)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault();

            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var tipo = Session["TipoRestaurante"].ToString();

            var tipoTienda = tipo;

            var tiendasSeleccionadas = db.TiendaArticulos.Where(t => t.ArticuloId == id).ToList();

            //var materialesCampaña = db.CampañaArticuloTMPs.Where(t => t.ArticuloKFCId == id && t.ArticuloKFC.EquityFranquicia == tipoTienda && t.Habilitado == true).OrderBy(t => t.TiendaId).ToList();

            var campaña = db.Campañas.Where(x => x.CampañaId == cam).FirstOrDefault();

            var campañaId = campaña.CampañaId;

            var articulosTMP = db.CampañaArticulos
                       .Where(x => x.ArticuloId == id)
                       .GroupBy(x => new
                       {
                           x.ArticuloId,
                           x.CampañaId,
                           x.Cantidad,
                           x.TiendaId,
                           x.Habilitado,
                       })
                       .Select(x => new MaterialesCampaña()
                       {
                           ArticuloId = x.Key.ArticuloId,
                           Campaña = campaña.Nombre,
                           CampañaId = x.Key.CampañaId,
                           Cantidad = x.Key.Cantidad,
                           TiendaId = x.Key.TiendaId,
                           Habilitado = x.Key.Habilitado,
                       });

            var tiendasCampaña = db.Tiendas
                            .GroupBy(x => new { x.NombreTienda, x.NoTienda, x.TiendaId })
                            .Select(x => new TiendasCampaña()
                            {
                                Restaurante = x.Key.NombreTienda,
                                TiendaId = x.Key.TiendaId,
                                CC = x.Key.NoTienda,
                                TipoTienda = tipoTienda,
                            });

            var materialesCampaña = articulosTMP.Join(tiendasCampaña,
                                 artCamp => artCamp.TiendaId,
                                 tienCamp => tienCamp.TiendaId,
                                 (artCamp, tienCamp) => new { tiendas = tienCamp, materiales = artCamp })
                            .Where(x => x.tiendas.TiendaId == x.materiales.TiendaId)
                            .Where(x => x.tiendas.TipoTienda == tipoTienda)
                            .GroupBy(x => new
                            {
                                x.tiendas.Restaurante,
                                x.tiendas.CC,
                                x.tiendas.TiendaId,
                                x.materiales.ArticuloId,
                                x.materiales.Articulo,
                                x.materiales.Cantidad,
                                x.materiales.Campaña,
                                x.materiales.CampañaId,
                                x.materiales.Habilitado,
                                x.tiendas.TipoTienda,
                            })
                                    .Select(x => new MaterialesTiendasCampaña()
                                    {
                                        ArticuloId = x.Key.ArticuloId,
                                        Articulo = x.Key.Articulo,
                                        Campaña = x.Key.Campaña,
                                        CampañaId = x.Key.CampañaId,
                                        Cantidad = x.Key.Cantidad,
                                        CC = x.Key.CC,
                                        Restaurante = x.Key.Restaurante,
                                        TiendaId = x.Key.TiendaId,
                                        Habilitado = x.Key.Habilitado,
                                    });

            var materialesTiendasCampaña = materialesCampaña.Where(m => m.Habilitado == true && m.CampañaId == campañaId).ToList();

            ViewBag.Material = db.Articulos.Where(x => x.ArticuloId == id).FirstOrDefault().Descripcion;

            return PartialView(materialesTiendasCampaña);
        }

        [AuthorizeUser(idOperacion: 2)]
        [HttpPost]
        public ActionResult Cantidades(FormCollection fc)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            string[] tiendaCampañaID = fc.GetValues("TiendaId");
            string[] articuloKFCId = fc.GetValues("ArticuloKFCId");
            string[] cantidadInput = fc.GetValues("CantidadInput");
            string[] campañaId = fc.GetValues("CampañaId");
            string[] seleccionado = fc.GetValues("Seleccionado");

            var cantidad = 0;
            var tiendaId = 0;
            var articuloId = 0;
            var campId = 0;

            for (var i = 0; i < tiendaCampañaID.Length; i++)
            {
                tiendaId = Convert.ToInt32(tiendaCampañaID[i]);
                articuloId = Convert.ToInt32(articuloKFCId[i]);
                cantidad = Convert.ToInt32(cantidadInput[i]);
                campId = Convert.ToInt32(campañaId[i]);

                CampañaArticulo campañaArticulo = db.CampañaArticulos.Where(ta => ta.TiendaId == tiendaId && ta.ArticuloId == articuloId && ta.CampañaId == campId).FirstOrDefault();

                if (campañaArticulo.Cantidad != cantidad)
                {
                    campañaArticulo.Cantidad = cantidad;

                    db.Entry(campañaArticulo).State = EntityState.Modified;

                    db.SaveChanges();
                }

            }

            var articulo = db.Articulos.Where(x => x.ArticuloId == articuloId).FirstOrDefault().Descripcion;

            movimiento = "Asignar Cantidades " + articulo;
            MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

            return Json(new { success = true, message = "CANTIDADES ASIGNADAS" }, JsonRequestBehavior.AllowGet);

        }

        [AuthorizeUser(idOperacion: 3)]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            var material = db.Articulos.Find(id);

            material.Eliminado = true;
            material.Activo = false;

            db.Entry(material).State = EntityState.Modified;
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                var campaña = db.Campañas.Where(x => x.Generada == "NO").FirstOrDefault();

                EliminarMateriales(id, campaña);

                movimiento = "Eliminar Artículo " + material.ArticuloId + " " + material.Descripcion + " / " + material.SencilloMultiple;
                MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                return Json(new { success = true, message = "ARTÍCULO ELIMINADO" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}