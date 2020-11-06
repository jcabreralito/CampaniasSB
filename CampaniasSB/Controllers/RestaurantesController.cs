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
    public class RestaurantesController : Controller
    {
        private readonly CampaniasContext db = new CampaniasContext();

        public string modulo = "Restaurantes";
        public string movimiento = string.Empty;

        public class spTiendasArticulos
        {
            public long TiendaArticuloId { get; set; }
            public int ArticuloKFCId { get; set; }
            public int TiendaId { get; set; }
            public bool Seleccionado { get; set; }
            public int CantidadDefault { get; set; }
            public string Restaurante { get; set; }
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
            public string Clasificacion { get; set; }
            public string CCoFranquicia { get; set; }
            public string Restaurante { get; set; }
            public string Region { get; set; }
            public string Ciudad { get; set; }
            public string Direccion { get; set; }
            public string Observaciones { get; set; }
            public bool Activo { get; set; }

            //00000000000000000000000000 GENERALES 0000000000000000000000000000000000

            public string Tipo { get; set; }
            public string NivelPrecio { get; set; }
            public bool MenuDigital { get; set; }
            public string CantidadDePantallas { get; set; }

            //00000000000000000000000000 POR PRODUCTO 0000000000000000000000000000000000

            public bool TerceraReceta { get; set; }
            public bool Arroz { get; set; }
            public bool Hamburgesas { get; set; }
            public bool Ensalada { get; set; }
            public bool PET2Litros { get; set; }
            public bool Postres { get; set; }
            public bool BisquetMiel { get; set; }
            public bool KeCono { get; set; }
            public bool KREAMBALL { get; set; }

            //00000000000000000000000000 MATERIALES ESPECIFICOS 0000000000000000000000000000000000

            public bool MenuBackLigth { get; set; }
            public bool Autoexpress { get; set; }
            public bool CopeteAERemodelado { get; set; }
            public bool CopeteAETradicional { get; set; }
            public bool PanelDeInnovacion { get; set; }
            public bool DisplayDeBurbuja { get; set; }
            public bool Delivery { get; set; }
            public bool MERCADO_DE_PRUEBA { get; set; }
            public bool AreaDeJuegos { get; set; }
            public bool COPETE_ESPECIAL_SOPORTE_LATERAL_4_VASOS { get; set; }
            public bool COPETE_ESPECIAL_SOPORTE_LATERAL_PET_2L { get; set; }
            public bool DisplayDePiso { get; set; }
            public bool WCNACIONAL67X100cm { get; set; }
            public bool AEMedidaEspecial { get; set; }
            public bool AEHolding { get; set; }
            public bool AECaribe { get; set; }
            public bool PanelComplementosHolding { get; set; }
            public bool PanelDeComplementosSinArrozSin3raReceta { get; set; }
            public bool PanelALaCartaCaribe { get; set; }
            public bool PanelALaCartaCaribeSin3raReceta { get; set; }
            public bool PanelALaCartaHolding { get; set; }
            public bool PanelALaCartaHoldingSin3raReceta { get; set; }
            public bool PanelDeComplementosDigital { get; set; }
            public bool PanelComplementosHoldingMR { get; set; }
            public bool Telefono { get; set; }
            public bool TelefonoPersonalizado { get; set; }
            public bool PanelKids { get; set; }
            public bool StickerNavidad { get; set; }

            //00000000000000000000000000 MEDIDAS ESPECIALES 0000000000000000000000000000000000
            public bool WCMedidaEspecial60_8x85cm { get; set; }
            public bool WC_MEDIDA_ESPECIAL_MALL_ORIENTE_100x120cm { get; set; }
            public bool WC_MEDIDA_ESPECIAL_ZUAZUA_87x120cm { get; set; }
            public bool WC_MEDIDA_ESPECIAL_CORREO_MAYOR_60x90cm { get; set; }
            public bool WC_MEDIDA_ESPECIAL_ZARAGOZA_90x100cm { get; set; }
            public bool MedidaEspecialPanelDeComplementos { get; set; }
            public bool MEDIDA_ESPECIAL_PRE_MENU_AE_SAN_ANTONIO_49x67_5cm { get; set; }
            public bool MEDIDA_ESPECIAL_AE_TECAMAC_48x67_5cm { get; set; }
            public bool MEDIDA_ESPECIAL_AE_VILLA_GARCIA_45x65cm { get; set; }
            public bool MEDIDA_ESPECIAL_AE_XOLA_49_9x66_9cm { get; set; }
            public bool MEDIDA_ESPECIAL_AE_ZUAZUA_51x71cm { get; set; }
            public bool MEDIDA_ESPECIAL_AE_VALLE_SOLEADO_51x71cm { get; set; }
            public bool MEDIDA_ESPECIAL_AE_MIRASIERRA_46x68cm { get; set; }
            public bool MEDIDA_ESPECIAL_AE_CELAYA_50x68_5cm { get; set; }
            public bool MEDIDA_ESPECIAL_AE_CANDILES_49_5x73_5cm { get; set; }
            public bool MEDIDA_BACKLIGHT_55_5X75_5CM { get; set; }
            public bool MEDIDA_BACKLIGHT_59_5X79CM { get; set; }
            public bool MEDIDAS_ESPECIALES_MENU { get; set; }

            //00000000000000000000000000 POR EQUIPO EN EL RESTAURANTE 0000000000000000000000000000000000

            public string TipoDeCaja { get; set; }
            public string AcomodoDeCajas { get; set; }
            public string NoMesaDeAreaComedor { get; set; }
            public string NoMesaDeAreaDeJuegos { get; set; }
            public string NumeroDeVentanas { get; set; }
            public string UbicacionPantallas { get; set; }

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

            var equityList = await db.Database.SqlQuery<spTiendasCaracteristicas>("spGetRestaurantesCaracteristicasEquity").ToListAsync();

            return PartialView(equityList);

            //return View();

        }

        // GET: Restaurantes
        [AuthorizeUser(idOperacion: 5)]
        public ActionResult IndexFQ()
        {
            Session["iconoTitulo"] = "fas fa-store";
            Session["titulo"] = "FRANQUICIAS";
            Session["homeB"] = string.Empty;
            Session["equityB"] = string.Empty;
            Session["franquiciasB"] = "active";
            Session["stockB"] = string.Empty;
            Session["restaurantesB"] = string.Empty;
            Session["materialesB"] = string.Empty;
            Session["campañasB"] = string.Empty;
            Session["caracteristicasB"] = string.Empty;

            var franquiciasList = db.Database.SqlQuery<spTiendasCaracteristicasFQ>("spGetRestaurantesCaracteristicasFranquicias").ToList();

            return PartialView(franquiciasList);

        }

        // GET: Restaurantes
        [AuthorizeUser(idOperacion: 5)]
        public ActionResult IndexSK()
        {
            Session["iconoTitulo"] = "fas fa-store";
            Session["titulo"] = "STOCK";
            Session["homeB"] = string.Empty;
            Session["equityB"] = string.Empty;
            Session["franquiciasB"] = string.Empty;
            Session["stockB"] = "active";
            Session["restaurantesB"] = string.Empty;
            Session["materialesB"] = string.Empty;
            Session["campañasB"] = string.Empty;
            Session["caracteristicasB"] = string.Empty;

            var stockList = db.Database.SqlQuery<spTiendasCaracteristicasSK>("spGetRestaurantesCaracteristicasStock").ToList();

            return PartialView(stockList);

        }

        // GET: Restaurantes
        [AuthorizeUser(idOperacion: 5)]
        public ActionResult IndexDev()
        {
            Session["iconoTitulo"] = "fas fa-store";
            Session["homeB"] = string.Empty;
            Session["equityB"] = "active";
            Session["franquiciasB"] = string.Empty;
            Session["stockB"] = string.Empty;
            Session["restaurantesB"] = string.Empty;
            Session["materialesB"] = string.Empty;
            Session["campañasB"] = string.Empty;
            Session["caracteristicasB"] = string.Empty;

            var equityList = db.Database.SqlQuery<spTiendasCaracteristicas>("spGetRestaurantesCaracteristicas").ToList();

            return PartialView(equityList);

        }

        public ActionResult GetDataEquity()
        {
            var equityList = db.Database.SqlQuery<spTiendas>("spGetRestaurantesEquity").ToList();

            return Json(new { data = equityList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataFranquicias()
        {
            var equityList = db.Database.SqlQuery<spTiendas>("spGetRestaurantesFranquicias").ToList();

            return Json(new { data = equityList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataStock()
        {
            var equityList = db.Database.SqlQuery<spTiendas>("spGetRestaurantesStock").ToList();

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
        public ActionResult AddOrEdit(int? cat, int id = 0)
        {
            var categoria = string.Empty;
            if (id == 0)
            {
                if (cat == 1)
                {
                    categoria = "EQUITY";
                }
                else if (cat == 2)
                {
                    categoria = "FRANQUICIAS";
                }
                else if (cat == 3)
                {
                    categoria = "STOCK";
                }

                Session["Categoria"] = categoria;

                ViewBag.RegionId = new SelectList(CombosHelper.GetRegiones(categoria, true), "RegionId", "Nombre");
                ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(categoria, true), "CiudadId", "Nombre");

                return PartialView(new Tienda());
            }
            else
            {

                var tipo = db.Tiendas.Where(x => x.TiendaId == id).FirstOrDefault().EquityFranquicia;
                Session["Categoria"] = tipo;

                var regionId = db.Tiendas.Where(x => x.TiendaId == id).FirstOrDefault().RegionId;
                var ciudadId = db.Tiendas.Where(x => x.TiendaId == id).FirstOrDefault().CiudadId;

                ViewBag.RegionId = new SelectList(CombosHelper.GetRegiones(tipo, true), "RegionId", "Nombre", regionId);
                ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(tipo, true), "CiudadId", "Nombre", ciudadId);

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
                var tipo = Session["Categoria"].ToString();

                tienda.Activo = true;
                tienda.EquityFranquicia = tipo;

                db.Tiendas.Add(tienda);
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    var cat = tienda.EquityFranquicia;

                    var categoria = string.Empty;

                    if (cat == "EQUITY" || cat == "STOCK")
                    {
                        categoria = "FRANQUICIAS";
                    }
                    else if (cat == "FRANQUICIAS")
                    {
                        categoria = "EQUITY";
                    }

                    var reglasList = db.Database.SqlQuery<ReglaCatalogo>("spGetReglasCatalogoCategoria @Categoria",
                    new SqlParameter("@Categoria", categoria)).ToList();

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

                    movimiento = "Agregar Restaurante " + tienda.TiendaId + " " + tienda.Restaurante + " / " + tienda.EquityFranquicia;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    //if (cat == "EQUITY")
                    //{
                    //    return RedirectToRoute("Index");
                    //}
                    //else if (cat == "FRANQUICIAS")
                    //{
                    //    return RedirectToRoute("IndexFQ");
                    //}
                    //else
                    //{
                    //    return RedirectToRoute("IndexSK");
                    //}
                    return Json(new { success = true, message = "RESTAURANTE AGREGADO" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //return RedirectToRoute("Index");
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var tipo = Session["Categoria"].ToString();

                tienda.EquityFranquicia = tipo;

                db.Entry(tienda).State = EntityState.Modified;
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    var cat = tienda.EquityFranquicia;

                    EliminarTodo(tienda);

                    MovimientosRestaurantes(tienda);

                    movimiento = "Actualizar Restaurante " + tienda.TiendaId + " " + tienda.Restaurante + " / " + tienda.EquityFranquicia;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);


                    //if (cat == "EQUITY")
                    //{
                    //    return RedirectToRoute("Index");
                    //}
                    //else if (cat == "FRANQUICIAS")
                    //{
                    //    return RedirectToRoute("IndexFQ");
                    //}
                    //else
                    //{
                    //    return RedirectToRoute("IndexSK");
                    //}
                    return Json(new { success = true, message = "RESTAURANTE ACTUALIZADO" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //return RedirectToRoute("Index");
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        private void MovimientosRestaurantes(Tienda tienda)
        {
            if (tienda.Activo == true)
            {
                //if (tienda.EquityFranquicia == "FRANQUICIAS")
                //{
                var response = MovementsHelper.AgregarTiendaArticulos(tienda.TiendaId);

                //==========================TODAS INICIO=======================

                //var tiendas = db.Tiendas.Where(x => x.EquityFranquicia == "FRANQUICIAS").ToList();

                //foreach (var tiendaInd in tiendas)
                //{
                //    var id = tiendaInd.TiendaId;

                //    var response = MovementsHelper.AgregarTiendaArticulosF(id);

                //    if (response.Succeeded)
                //    {
                //        var campaña = db.Campañas.Where(x => x.Generada == "NO").ToList();
                //        if (campaña.Count >= 1)
                //        {
                //            MovementsHelper.AgregarArticuloPorTiendas(tiendaInd);
                //        }
                //    }
                //}

                //==========================TODAS FIN=======================

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
                //}
                //else
                //{
                //    var response = MovementsHelper.AgregarTiendaArticulos(tienda.TiendaId);

                //    //==========================TODAS INICIO=======================

                //    //var tiendas = db.Tiendas.Where(x => x.EquityFranquicia == "EQUITY").ToList();

                //    //foreach (var tiendaInd in tiendas)
                //    //{
                //    //    var id = tiendaInd.TiendaId;

                //    //    var response = MovementsHelper.AgregarTiendaArticulos(id);

                //    //    if (response.Succeeded)
                //    //    {
                //    //        var campaña = db.Campañas.Where(x => x.Generada == "NO").ToList();
                //    //        if (campaña.Count >= 1)
                //    //        {
                //    //            MovementsHelper.AgregarArticuloPorTiendas(tiendaInd);
                //    //        }
                //    //    }
                //    //}

                //    //==========================TODAS FIN=======================

                //    if (response.Succeeded)
                //    {
                //        var campaña = db.Campañas.Where(x => x.Generada == "NO").ToList();
                //        if (campaña.Count >= 1)
                //        {
                //            var campañaid = campaña.FirstOrDefault().CampañaId;

                //            MovementsHelper.AgregarArticuloPorTiendas(tienda.TiendaId, campañaid);
                //        }
                //    }
                //}

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

        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult EditCG(int id)
        {

            var tipoId = db.Tiendas.Where(x => x.TiendaId == id).FirstOrDefault().TipoId;
            var nivelPrecioId = db.Tiendas.Where(x => x.TiendaId == id).FirstOrDefault().NuevoNivelDePrecioId;

            if (tipoId == 0)
            {
                ViewBag.TipoId = new SelectList(CombosHelper.GetTiposTienda(true), "TipoTiendaId", "Tipo");
            }
            else
            {
                ViewBag.TipoId = new SelectList(CombosHelper.GetTiposTienda(true), "TipoTiendaId", "Tipo", tipoId);
            }

            if (nivelPrecioId == 0)
            {
                ViewBag.NuevoNivelDePrecioId = new SelectList(CombosHelper.GetNivelesPrecio(true), "NivelPrecioId", "Descripcion");
            }
            else
            {
                ViewBag.NuevoNivelDePrecioId = new SelectList(CombosHelper.GetNivelesPrecio(true), "NivelPrecioId", "Descripcion", nivelPrecioId);
            }

            return PartialView(db.Tiendas.Where(x => x.TiendaId == id).FirstOrDefault());

        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult EditCG(Tienda tiendas, FormCollection formCollection)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            db.Entry(tiendas).State = EntityState.Modified;
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                EliminarTodo(tiendas);

                MovimientosRestaurantes(tiendas);

                movimiento = "Actualizar CG " + tiendas.TiendaId + " " + tiendas.Restaurante + " / " + tiendas.EquityFranquicia;
                MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                return Json(new { success = true, message = "RESTAURANTE ACTUALIZADO" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult EditCGDev(int id)
        {
            var restauranteCG = db.Database.SqlQuery<spRestaurantesCaracteristicas>("spGetTiendasCaracteristicasG @TiendaID",
                                new SqlParameter("@TiendaId", id)).ToList();

            var tipo = restauranteCG.Where(x => x.ReglaCatalogoId == 1 || x.ReglaCatalogoId == 104).FirstOrDefault().Valor;
            var tipoId = db.TipoTiendas.Where(x => x.Tipo == tipo).FirstOrDefault().TipoTiendaId;
            var nivel = restauranteCG.Where(x => x.ReglaCatalogoId == 5 || x.ReglaCatalogoId == 113).FirstOrDefault().Valor;
            var nivelPrecioId = db.NivelPrecios.Where(x => x.Descripcion == nivel).FirstOrDefault().NivelPrecioId;

            if (tipoId == 0)
            {
                ViewBag.TipoId = new SelectList(CombosHelper.GetTiposTienda(true), "TipoTiendaId", "Tipo");
            }
            else
            {
                ViewBag.TipoId = new SelectList(CombosHelper.GetTiposTienda(true), "TipoTiendaId", "Tipo", tipoId);
            }

            if (nivelPrecioId == 0)
            {
                ViewBag.NuevoNivelDePrecioId = new SelectList(CombosHelper.GetNivelesPrecio(true), "NivelPrecioId", "Descripcion");
            }
            else
            {
                ViewBag.NuevoNivelDePrecioId = new SelectList(CombosHelper.GetNivelesPrecio(true), "NivelPrecioId", "Descripcion", nivelPrecioId);
            }

            return PartialView(restauranteCG);
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult EditCGDev(Tienda tiendas, FormCollection formCollection)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            string[] restauranteId = formCollection.GetValues("TiendaId");
            string[] catalogoId = formCollection.GetValues("ReglaCatalogoId");
            string[] tipo = formCollection.GetValues("TipoId");
            string[] nivel = formCollection.GetValues("NuevoNivelDePrecioId");
            string[] valorMD = formCollection.GetValues("ValorMD");
            string[] valorP = formCollection.GetValues("ValorP");

            for (var i = 0; i < restauranteId.Length; i++)
            {
                var tiendaId = Convert.ToInt32(restauranteId[i]);
                var reglaCatalogoId = Convert.ToInt32(catalogoId[i]);

                if (catalogoId[i] == "1")
                {
                    var tipoId = Convert.ToInt32(tipo[0]);

                    var tipoValor = db.TipoTiendas.Where(x => x.TipoTiendaId == tipoId).FirstOrDefault().Tipo;

                    db.Database.ExecuteSqlCommand(
                    "spActualizarTiendaCaracteristicas @ReglaCatalogoId, @TiendaId, @Valor",
                    new SqlParameter("@ReglaCatalogoId", reglaCatalogoId),
                    new SqlParameter("@TiendaId", tiendaId),
                    new SqlParameter("@Valor", tipoValor));

                }
                else if (catalogoId[i] == "5")
                {
                    var nivelId = Convert.ToInt32(nivel[0]);

                    var tipoValor = db.NivelPrecios.Where(x => x.NivelPrecioId == nivelId).FirstOrDefault().Descripcion;

                    db.Database.ExecuteSqlCommand(
                    "spActualizarTiendaCaracteristicas @ReglaCatalogoId, @TiendaId, @Valor",
                    new SqlParameter("@ReglaCatalogoId", reglaCatalogoId),
                    new SqlParameter("@TiendaId", tiendaId),
                    new SqlParameter("@Valor", tipoValor));

                }
                else if (catalogoId[i] == "21")
                {
                    var tipoValor = "NO";

                    if (valorMD == null)
                    {
                        tipoValor = "NO";
                    }
                    else
                    {
                        tipoValor = "SI";
                    }

                    db.Database.ExecuteSqlCommand(
                    "spActualizarTiendaCaracteristicas @ReglaCatalogoId, @TiendaId, @Valor",
                    new SqlParameter("@ReglaCatalogoId", reglaCatalogoId),
                    new SqlParameter("@TiendaId", tiendaId),
                    new SqlParameter("@Valor", tipoValor));

                }
                else if (catalogoId[i] == "22")
                {
                    var tipoValor = valorP[0];

                    db.Database.ExecuteSqlCommand(
                    "spActualizarTiendaCaracteristicas @ReglaCatalogoId, @TiendaId, @Valor",
                    new SqlParameter("@ReglaCatalogoId", reglaCatalogoId),
                    new SqlParameter("@TiendaId", tiendaId),
                    new SqlParameter("@Valor", tipoValor));

                }

            }

            var tiendaIdA = Convert.ToInt32(restauranteId[0]);

            var tienda = db.Tiendas.Where(x => x.TiendaId == tiendaIdA).FirstOrDefault();

            EliminarTodo(tienda);

            MovimientosRestaurantes(tienda);


            movimiento = "Actualizar CG " + tiendas.TiendaId + " " + tiendas.Restaurante + " / " + tiendas.EquityFranquicia;
            MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

            return Json(new { success = true, message = "RESTAURANTE ACTUALIZADO" }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult EditCPDev(int id)
        {
            var restauranteCP = db.Database.SqlQuery<spRestaurantesCaracteristicas>("spGetTiendasCaracteristicasP @TiendaID",
                                new SqlParameter("@TiendaId", id)).ToList();

            return PartialView(restauranteCP);
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult EditCPDev(Tienda tiendas, FormCollection formCollection)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            string[] restauranteId = formCollection.GetValues("TiendaId");
            string[] catalogoId = formCollection.GetValues("ReglaCatalogoId");
            string[] valor = formCollection.GetValues("Valor");

            for (var i = 0; i < restauranteId.Length; i++)
            {
                var tiendaId = Convert.ToInt32(restauranteId[i]);
                var reglaCatalogoId = Convert.ToInt32(catalogoId[i]);

                var tipoValor = "NO";

                if (valor == null)
                {
                    db.Database.ExecuteSqlCommand(
                    "spActualizarTiendaCaracteristicas @ReglaCatalogoId, @TiendaId, @Valor",
                    new SqlParameter("@ReglaCatalogoId", reglaCatalogoId),
                    new SqlParameter("@TiendaId", tiendaId),
                    new SqlParameter("@Valor", tipoValor));
                }
                else
                {
                    for (int v = 0; v < valor.Length; v++)
                    {
                        if (catalogoId[i] == valor[v])
                        {
                            tipoValor = "SI";

                            db.Database.ExecuteSqlCommand(
                            "spActualizarTiendaCaracteristicas @ReglaCatalogoId, @TiendaId, @Valor",
                            new SqlParameter("@ReglaCatalogoId", reglaCatalogoId),
                            new SqlParameter("@TiendaId", tiendaId),
                            new SqlParameter("@Valor", tipoValor));

                            break;
                        }
                        db.Database.ExecuteSqlCommand(
                        "spActualizarTiendaCaracteristicas @ReglaCatalogoId, @TiendaId, @Valor",
                        new SqlParameter("@ReglaCatalogoId", reglaCatalogoId),
                        new SqlParameter("@TiendaId", tiendaId),
                        new SqlParameter("@Valor", tipoValor));
                    }
                }

            }

            var tiendaIdA = Convert.ToInt32(restauranteId[0]);

            var tienda = db.Tiendas.Where(x => x.TiendaId == tiendaIdA).FirstOrDefault();

            EliminarTodo(tienda);

            MovimientosRestaurantes(tienda);


            movimiento = "Actualizar CP " + tiendas.TiendaId + " " + tiendas.Restaurante + " / " + tiendas.EquityFranquicia;
            MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

            return Json(new { success = true, message = "RESTAURANTE ACTUALIZADO" }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult EditCP(int id)
        {

            return PartialView(db.Tiendas.Where(x => x.TiendaId == id).FirstOrDefault());
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult EditCP(Tienda tienda)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            db.Entry(tienda).State = EntityState.Modified;
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                EliminarTodo(tienda);

                MovimientosRestaurantes(tienda);

                movimiento = "Actualizar CP " + tienda.TiendaId + " " + tienda.Restaurante + " / " + tienda.EquityFranquicia;
                MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                return Json(new { success = true, message = "RESTAURANTE ACTUALIZADO" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult EditCME(int id)
        {

            return PartialView(db.Tiendas.Where(x => x.TiendaId == id).FirstOrDefault());
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult EditCMEDev(int id)
        {
            var restauranteCME = db.Database.SqlQuery<spRestaurantesCaracteristicas>("spGetTiendasCaracteristicasME @TiendaID",
                                new SqlParameter("@TiendaId", id)).ToList();

            return PartialView(restauranteCME);
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult EditCMEDev(Tienda tiendas, FormCollection formCollection)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            string[] restauranteId = formCollection.GetValues("TiendaId");
            string[] catalogoId = formCollection.GetValues("ReglaCatalogoId");
            string[] valor = formCollection.GetValues("Valor");

            for (var i = 0; i < restauranteId.Length; i++)
            {
                var tiendaId = Convert.ToInt32(restauranteId[i]);
                var reglaCatalogoId = Convert.ToInt32(catalogoId[i]);

                var tipoValor = "NO";

                if (valor == null)
                {
                    db.Database.ExecuteSqlCommand(
                    "spActualizarTiendaCaracteristicas @ReglaCatalogoId, @TiendaId, @Valor",
                    new SqlParameter("@ReglaCatalogoId", reglaCatalogoId),
                    new SqlParameter("@TiendaId", tiendaId),
                    new SqlParameter("@Valor", tipoValor));
                }
                else
                {
                    for (int v = 0; v < valor.Length; v++)
                    {
                        if (catalogoId[i] == valor[v])
                        {
                            tipoValor = "SI";

                            db.Database.ExecuteSqlCommand(
                            "spActualizarTiendaCaracteristicas @ReglaCatalogoId, @TiendaId, @Valor",
                            new SqlParameter("@ReglaCatalogoId", reglaCatalogoId),
                            new SqlParameter("@TiendaId", tiendaId),
                            new SqlParameter("@Valor", tipoValor));

                            break;
                        }
                        db.Database.ExecuteSqlCommand(
                        "spActualizarTiendaCaracteristicas @ReglaCatalogoId, @TiendaId, @Valor",
                        new SqlParameter("@ReglaCatalogoId", reglaCatalogoId),
                        new SqlParameter("@TiendaId", tiendaId),
                        new SqlParameter("@Valor", tipoValor));
                    }
                }

            }

            var tiendaIdA = Convert.ToInt32(restauranteId[0]);

            var tienda = db.Tiendas.Where(x => x.TiendaId == tiendaIdA).FirstOrDefault();

            EliminarTodo(tienda);

            MovimientosRestaurantes(tienda);


            movimiento = "Actualizar ME " + tiendas.TiendaId + " " + tiendas.Restaurante + " / " + tiendas.EquityFranquicia;
            MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

            return Json(new { success = true, message = "RESTAURANTE ACTUALIZADO" }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult EditCME(Tienda tienda)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            db.Entry(tienda).State = EntityState.Modified;
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                EliminarTodo(tienda);

                MovimientosRestaurantes(tienda);

                movimiento = "Actualizar CME " + tienda.TiendaId + " " + tienda.Restaurante + " / " + tienda.EquityFranquicia;
                MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                return Json(new { success = true, message = "RESTAURANTE ACTUALIZADO" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult EditCMES(int id)
        {
            return PartialView(db.Tiendas.Where(x => x.TiendaId == id).FirstOrDefault());
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult EditCMESDev(int id)
        {
            var restauranteCMES = db.Database.SqlQuery<spRestaurantesCaracteristicas>("spGetTiendasCaracteristicasMED @TiendaID",
                                new SqlParameter("@TiendaId", id)).ToList();

            return PartialView(restauranteCMES);
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult EditCMESDev(Tienda tiendas, FormCollection formCollection)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            string[] restauranteId = formCollection.GetValues("TiendaId");
            string[] catalogoId = formCollection.GetValues("ReglaCatalogoId");
            string[] valor = formCollection.GetValues("Valor");

            for (var i = 0; i < restauranteId.Length; i++)
            {
                var tiendaId = Convert.ToInt32(restauranteId[i]);
                var reglaCatalogoId = Convert.ToInt32(catalogoId[i]);

                var tipoValor = "NO";

                if (valor == null)
                {
                    db.Database.ExecuteSqlCommand(
                    "spActualizarTiendaCaracteristicas @ReglaCatalogoId, @TiendaId, @Valor",
                    new SqlParameter("@ReglaCatalogoId", reglaCatalogoId),
                    new SqlParameter("@TiendaId", tiendaId),
                    new SqlParameter("@Valor", tipoValor));
                }
                else
                {
                    for (int v = 0; v < valor.Length; v++)
                    {
                        if (catalogoId[i] == valor[v])
                        {
                            tipoValor = "SI";

                            db.Database.ExecuteSqlCommand(
                            "spActualizarTiendaCaracteristicas @ReglaCatalogoId, @TiendaId, @Valor",
                            new SqlParameter("@ReglaCatalogoId", reglaCatalogoId),
                            new SqlParameter("@TiendaId", tiendaId),
                            new SqlParameter("@Valor", tipoValor));

                            break;
                        }
                        db.Database.ExecuteSqlCommand(
                        "spActualizarTiendaCaracteristicas @ReglaCatalogoId, @TiendaId, @Valor",
                        new SqlParameter("@ReglaCatalogoId", reglaCatalogoId),
                        new SqlParameter("@TiendaId", tiendaId),
                        new SqlParameter("@Valor", tipoValor));
                    }
                }

            }

            var tiendaIdA = Convert.ToInt32(restauranteId[0]);

            var tienda = db.Tiendas.Where(x => x.TiendaId == tiendaIdA).FirstOrDefault();

            EliminarTodo(tienda);

            MovimientosRestaurantes(tienda);


            movimiento = "Actualizar CMES " + tiendas.TiendaId + " " + tiendas.Restaurante + " / " + tiendas.EquityFranquicia;
            MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

            return Json(new { success = true, message = "RESTAURANTE ACTUALIZADO" }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult EditCMES(Tienda tienda)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            db.Entry(tienda).State = EntityState.Modified;
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                EliminarTodo(tienda);

                MovimientosRestaurantes(tienda);

                movimiento = "Actualizar CMES " + tienda.TiendaId + " " + tienda.Restaurante + " / " + tienda.EquityFranquicia;
                MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                return Json(new { success = true, message = "RESTAURANTE ACTUALIZADO" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult EditCER(int id)
        {

            var tipocajaId = db.Tiendas.Where(x => x.TiendaId == id).FirstOrDefault().TipoDeCajaId;
            var acomodoCaja = db.Tiendas.Where(x => x.TiendaId == id).FirstOrDefault().AcomodoDeCajas;

            ViewBag.TipoDeCajaId = new SelectList(CombosHelper.GetTiposCaja(true), "TipoCajaId", "Descripcion", tipocajaId);
            ViewBag.AcomodoDeCajas = new SelectList(CombosHelper.GetAcomodoCajas(true), "AcomodoCajaId", "Descripcion", acomodoCaja);

            return PartialView(db.Tiendas.Where(x => x.TiendaId == id).FirstOrDefault());
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult EditCERDev(int id)
        {
            var restauranteCER = db.Database.SqlQuery<spRestaurantesCaracteristicas>("spGetTiendasCaracteristicasER @TiendaID",
                                new SqlParameter("@TiendaId", id)).ToList();

            return PartialView(restauranteCER);
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult EditCERDev(Tienda tiendas, FormCollection formCollection)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            string[] restauranteId = formCollection.GetValues("TiendaId");
            string[] catalogoId = formCollection.GetValues("ReglaCatalogoId");
            string[] valor = formCollection.GetValues("ValorMD");
            string[] valorP = formCollection.GetValues("ValorP");

            for (var i = 0; i < restauranteId.Length; i++)
            {
                var tiendaId = Convert.ToInt32(restauranteId[i]);
                var reglaCatalogoId = Convert.ToInt32(catalogoId[i]);

                var tipoValor = "NO";

                if (restauranteId.Length == valorP.Length)
                {
                    tipoValor = valorP[i].ToUpper();

                    db.Database.ExecuteSqlCommand(
                    "spActualizarTiendaCaracteristicas @ReglaCatalogoId, @TiendaId, @Valor",
                    new SqlParameter("@ReglaCatalogoId", reglaCatalogoId),
                    new SqlParameter("@TiendaId", tiendaId),
                    new SqlParameter("@Valor", tipoValor));

                }
                else
                {
                    for (int vp = i; vp < valorP.Length; vp++)
                    {
                        tipoValor = valorP[i].ToUpper();

                        db.Database.ExecuteSqlCommand(
                        "spActualizarTiendaCaracteristicas @ReglaCatalogoId, @TiendaId, @Valor",
                        new SqlParameter("@ReglaCatalogoId", reglaCatalogoId),
                        new SqlParameter("@TiendaId", tiendaId),
                        new SqlParameter("@Valor", tipoValor));
                    }

                    if (valor == null)
                    {
                        db.Database.ExecuteSqlCommand(
                        "spActualizarTiendaCaracteristicas @ReglaCatalogoId, @TiendaId, @Valor",
                        new SqlParameter("@ReglaCatalogoId", reglaCatalogoId),
                        new SqlParameter("@TiendaId", tiendaId),
                        new SqlParameter("@Valor", tipoValor));
                    }
                    else
                    {
                        for (int v = 0; v < valor.Length; v++)
                        {
                            if (catalogoId[i] == valor[v])
                            {
                                tipoValor = "SI";

                                db.Database.ExecuteSqlCommand(
                                "spActualizarTiendaCaracteristicas @ReglaCatalogoId, @TiendaId, @Valor",
                                new SqlParameter("@ReglaCatalogoId", reglaCatalogoId),
                                new SqlParameter("@TiendaId", tiendaId),
                                new SqlParameter("@Valor", tipoValor));

                                break;
                            }
                            db.Database.ExecuteSqlCommand(
                            "spActualizarTiendaCaracteristicas @ReglaCatalogoId, @TiendaId, @Valor",
                            new SqlParameter("@ReglaCatalogoId", reglaCatalogoId),
                            new SqlParameter("@TiendaId", tiendaId),
                            new SqlParameter("@Valor", tipoValor));
                        }
                    }
                }


            }

            var tiendaIdA = Convert.ToInt32(restauranteId[0]);

            var tienda = db.Tiendas.Where(x => x.TiendaId == tiendaIdA).FirstOrDefault();

            EliminarTodo(tienda);

            MovimientosRestaurantes(tienda);


            movimiento = "Actualizar CG " + tiendas.TiendaId + " " + tiendas.Restaurante + " / " + tiendas.EquityFranquicia;
            MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

            return Json(new { success = true, message = "RESTAURANTE ACTUALIZADO" }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult EditCER(Tienda tienda)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            db.Entry(tienda).State = EntityState.Modified;
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                EliminarTodo(tienda);

                MovimientosRestaurantes(tienda);

                movimiento = "Actualizar CER " + tienda.TiendaId + " " + tienda.Restaurante + " / " + tienda.EquityFranquicia;
                MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                return Json(new { success = true, message = "RESTAURANTE ACTUALIZADO" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //[AuthorizeUser(idOperacion: 2)]
        //[HttpGet]
        //public ActionResult Materiales2(int id)
        //{
        //    Session["TiendaIdMaterial"] = id;

        //    var materialesList = db.Database.SqlQuery<spTiendasArticulos>("spGetTiendaArticulos @TiendaId",
        //        new SqlParameter("@TiendaID", id)).ToList();


        //    ViewBag.Tienda = db.Tiendas.Where(t => t.TiendaId == id).FirstOrDefault().Restaurante;

        //    return PartialView(materialesList);
        //}

        //[AuthorizeUser(idOperacion: 2)]
        //[HttpPost]
        //public ActionResult Materiales2(int id, bool sel)
        //{
        //    var material = db.TiendaArticulos.Where(x => x.TiendaArticuloId == id).FirstOrDefault();

        //    material.Seleccionado = sel;

        //    db.Entry(material).State = EntityState.Modified;
        //    var response = DBHelper.SaveChanges(db);
        //    if (response.Succeeded)
        //    {
        //        return Json(new { success = true, message = "MATERIAL ACTUALIZADO" }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

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

            ViewBag.Restaurante = db.Tiendas.Where(t => t.TiendaId == id).FirstOrDefault().Restaurante;

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
                    var articuloId = tiendaArticulo.ArticuloKFCId;

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
                        CampañaArticulo campañaArticulo = db.CampañaArticulos.Where(ta => ta.TiendaId == tiendaId && ta.ArticuloKFCId == articuloId && ta.CampañaId == campId).FirstOrDefault();

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

                                        var articuloCantidadDefault = db.Articulos.Where(a => a.ArticuloKFCId == campañaArticulo.ArticuloKFCId).FirstOrDefault().CantidadDefault;

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
                        var articuloId = tiendaArticulo.ArticuloKFCId;

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
                            CampañaArticulo campañaArticulo = db.CampañaArticulos.Where(ta => ta.TiendaId == tiendaId && ta.ArticuloKFCId == articuloId && ta.CampañaId == campId).FirstOrDefault();

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

                                            var articuloCantidadDefault = db.Articulos.Where(a => a.ArticuloKFCId == campañaArticulo.ArticuloKFCId).FirstOrDefault().CantidadDefault;

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



                        //if (campañaArticulo != null)
                        //{
                        //    if (seleccionado == null)
                        //    {
                        //        selec = false;
                        //        cantidad = 0;

                        //        tiendaArticulo.Seleccionado = selec;

                        //        db.Entry(tiendaArticulo).State = EntityState.Modified;

                        //        campañaArticulo.Habilitado = selec;
                        //        campañaArticulo.Cantidad = cantidad;

                        //        db.Entry(campañaArticulo).State = EntityState.Modified;

                        //        db.SaveChanges();
                        //    }
                        //    else
                        //    {
                        //        for (var j = 0; j < seleccionado.Length; j++)
                        //        {
                        //            if (articuloKFCTMPId[i] == seleccionado[j])
                        //            {
                        //                selec = true;

                        //                tiendaArticulo.Seleccionado = selec;

                        //                db.Entry(tiendaArticulo).State = EntityState.Modified;

                        //                var articuloCantidadDefault = db.ArticuloKFCs.Where(a => a.ArticuloKFCId == campañaArticulo.ArticuloKFCId).FirstOrDefault().CantidadDefault;

                        //                cantidad = articuloCantidadDefault;
                        //                campañaArticulo.Cantidad = cantidad;
                        //                campañaArticulo.Habilitado = selec;

                        //                db.Entry(campañaArticulo).State = EntityState.Modified;
                        //                db.SaveChanges();

                        //                break;
                        //            }
                        //        }
                        //        if (!selec)
                        //        {
                        //            selec = false;
                        //            cantidad = 0;

                        //            tiendaArticulo.Seleccionado = selec;

                        //            db.Entry(tiendaArticulo).State = EntityState.Modified;

                        //            campañaArticulo.Habilitado = selec;
                        //            campañaArticulo.Cantidad = cantidad;

                        //            db.Entry(campañaArticulo).State = EntityState.Modified;

                        //            db.SaveChanges();
                        //        }
                        //    }
                        //}

                    }
                }
            }
            TiendaArticulo tienda = db.TiendaArticulos.Find(Convert.ToInt32(articuloKFCTMPId[0]));

            movimiento = "Asignar Materiales / " + tienda.TiendaId + " / " + tienda.Tienda.Restaurante;
            MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

            return Json(new { success = true, message = "MATERIALES ASIGNADOS" }, JsonRequestBehavior.AllowGet);

        }

        //[AuthorizeUser(idOperacion: 1)]
        //[HttpGet]
        //public ActionResult AsignarCaracteristicaG(int id)
        //{
        //    var tipo = db.Tiendas.Where(x => x.TiendaId == id).FirstOrDefault().EquityFranquicia;
        //    ViewBag.TiendaId = id;

        //    ViewBag.TiendaConfiguracionId = new SelectList(CombosHelper.GetConfiguracionesG(id, tipo, true), "TiendaConfiguracionId", "Nombre");

        //    return PartialView(new AsignarConfiguracionTienda());
        //}

        //[AuthorizeUser(idOperacion: 1)]
        //[HttpPost]
        //public ActionResult AsignarCaracteristicaG(AsignarConfiguracionTienda asignarConfiguracionTienda)
        //{
        //    db.AsignarConfiguracionTiendas.Add(asignarConfiguracionTienda);
        //    var response = DBHelper.SaveChanges(db);
        //    if (response.Succeeded)
        //    {
        //        return Json(new { success = true, message = "CARACTERÍSTICA ASIGNADA" }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

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

                movimiento = "Eliminar Restaurante " + tienda.TiendaId + " " + tienda.Restaurante + " / " + tienda.EquityFranquicia;
                MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                return Json(new { success = true, message = "RESTAURANTE ELIMINADO" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //public void ExportCSV()
        //{
        //}

        //public ActionResult ExportExcel()
        //{
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //    var tipo = "EQUITY";
        //    var restaurantes = db.Tiendas.Where(x => x.Activo == true && x.EquityFranquicia == tipo).ToList();
        //    using (var package = new ExcelPackage())
        //    {
        //        var worksheet = package.Workbook.Worksheets.Add("Restaurantes");
        //        var tableBody = worksheet.Cells["A1:A1"].LoadFromCollection(
        //            from f in restaurantes
        //            select new { f.Restaurante, f.CCoFranquicia, f.Region.Nombre, f.Clasificacion, f.Observaciones, f.Direccion }, true);
        //        var header = worksheet.Cells["A1:F1"];
        //        worksheet.Cells["A1:F1"].AutoFitColumns();
        //        tableBody.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        tableBody.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        tableBody.Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
        //        tableBody.Style.Border.BorderAround(ExcelBorderStyle.Medium);
        //        header.Style.Font.Bold = true;
        //        header.Style.Font.Color.SetColor(Color.White);
        //        header.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        header.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);

        //        for (int i = 1; i <= worksheet.Dimension.End.Column; i++)
        //        {
        //            worksheet.Column(i).AutoFit();
        //        }

        //        string path = @"C:\Restaurantes.xlsx";
        //        var fi1 = new FileInfo(path);

        //        package.SaveAs(fi1);

        //        return Json(new { success = true, message = "Archivo Exportado" }, JsonRequestBehavior.AllowGet);



        //        //await IJSExtensions.GuardarComo("Restaurantes.xlsx", package.GetAsByteArray());

        //    }
        //}
    }
}