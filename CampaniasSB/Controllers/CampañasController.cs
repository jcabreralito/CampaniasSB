using CampaniasSB.Classes;
using CampaniasSB.Filters;
using CampaniasSB.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace CampaniasSB.Controllers
{
    [Authorize]
    public class CampañasController : Controller
    {
        private readonly CampaniasContext db = new CampaniasContext();

        public string modulo = "Campañas";
        public string movimiento = string.Empty;

        public class MaterialesTotales
        {
            public string ArticuloKFC { get; set; }
            public string Campaña { get; set; }
            public double Cantidad { get; set; }
            public string Proveedor { get; set; }
            public string Imagen { get; set; }
            public int TiendaId { get; set; }

            [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
            public double TotalCantidad { get; set; }
            public string EquityFranquicia { get; set; }

        }

        public class MaterialesTotalesOrdenes
        {
            public int ArticuloKFCId { get; set; }
            public string Campaña { get; set; }
            public int TiendaId { get; set; }

            [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
            public double Cantidad { get; set; }

        }

        public class CodigosMateriales
        {
            public string ArticuloKFC { get; set; }
            public string Campaña { get; set; }
            public double Codigo { get; set; }
        }

        public class CodigosMaterialesOrdenes
        {
            public int ArticuloKFCId { get; set; }
            public string Campaña { get; set; }
            public double Codigo { get; set; }
        }

        public class CodigosMaterialesTotalOrdenes
        {
            public string Descripcion { get; set; }
            public string Campaña { get; set; }
            public int Codigo { get; set; }
            [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
            public double Cantidad { get; set; }
            public int TiendaId { get; set; }
            public string CCoFranquicia { get; set; }
            public int CampañaId { get; set; }
        }

        public class CodigosMaterialesTotal
        {
            public string ArticuloKFC { get; set; }
            public string Campaña { get; set; }
            public int Codigo { get; set; }
            public string Proveedor { get; set; }
            [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
            public double TotalCantidad { get; set; }
            public int CampañaId { get; set; }
            public string Familia { get; set; }

        }

        public class MaterialesCampaña
        {
            public int ArticuloKFCId { get; set; }
            public string ArticuloKFC { get; set; }
            public string Campaña { get; set; }
            public int CampañaId { get; set; }
            public double Cantidad { get; set; }
            public int TiendaId { get; set; }
            public bool Habilitado { get; set; }
            public int ProveedorId { get; set; }
            public int FamiliaId { get; set; }
        }

        public class spTiendasActivas
        {
            public int TiendaId { get; set; }
            public string CCoFranquicia { get; set; }
            public string Restaurante { get; set; }
            public string Region { get; set; }
            public string Ciudad { get; set; }
            public string EquityFranquicia { get; set; }
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

            ////00000000000000000000000000 GENERALES 0000000000000000000000000000000000
            //public string Tipo { get; set; }
            //public string NuevoNivelDePrecio { get; set; }
            //public bool MenuDigital { get; set; }
            //public string CantidadDePantallas { get; set; }
            ////00000000000000000000000000 POR PRODUCTO 0000000000000000000000000000000000
            //public bool TerceraReceta { get; set; }
            //public bool Arroz { get; set; }
            //public bool Hamburgesas { get; set; }
            //public bool Ensalada { get; set; }
            //public bool PET2Litros { get; set; }
            //public bool Postres { get; set; }
            //public bool BisquetMiel { get; set; }
            //public bool KeCono { get; set; }
            //public bool KREAMBALL { get; set; }

            ////00000000000000000000000000 MATERIALES ESPECIFICOS 0000000000000000000000000000000000

            //public bool MenuBackLigth { get; set; }
            //public bool Autoexpress { get; set; }
            //public bool CopeteAERemodelado { get; set; }
            //public bool CopeteAETradicional { get; set; }
            //public bool PanelDeInnovacion { get; set; }
            //public bool DisplayDeBurbuja { get; set; }
            //public bool Delivery { get; set; }
            //public bool MERCADO_DE_PRUEBA { get; set; }
            //public bool AreaDeJuegos { get; set; }
            //public bool COPETE_ESPECIAL_SOPORTE_LATERAL_4_VASOS { get; set; }
            //public bool COPETE_ESPECIAL_SOPORTE_LATERAL_PET_2L { get; set; }
            //public bool DisplayDePiso { get; set; }
            //public bool WCNACIONAL67X100cm { get; set; }
            //public bool AEMedidaEspecial { get; set; }
            //public bool AEHolding { get; set; }
            //public bool AECaribe { get; set; }
            //public bool PanelComplementosHolding { get; set; }
            //public bool PanelDeComplementosSinArrozSin3raReceta { get; set; }
            //public bool PanelALaCartaCaribe { get; set; }
            //public bool PanelALaCartaCaribeSin3raReceta { get; set; }
            //public bool PanelALaCartaHolding { get; set; }
            //public bool PanelALaCartaHoldingSin3raReceta { get; set; }
            //public bool PanelDeComplementosDigital { get; set; }
            //public bool PanelComplementosHoldingMR { get; set; }
            //public bool Telefono { get; set; }

            ////00000000000000000000000000 MEDIDAS ESPECIALES 0000000000000000000000000000000000
            //public bool WCMedidaEspecial60_8x85cm { get; set; }
            //public bool WC_MEDIDA_ESPECIAL_MALL_ORIENTE_100x120cm { get; set; }
            //public bool WC_MEDIDA_ESPECIAL_ZUAZUA_87x120cm { get; set; }
            //public bool WC_MEDIDA_ESPECIAL_CORREO_MAYOR_60x90cm { get; set; }
            //public bool WC_MEDIDA_ESPECIAL_ZARAGOZA_90x100cm { get; set; }
            //public bool MedidaEspecialPanelDeComplementos { get; set; }
            //public bool MEDIDA_ESPECIAL_PRE_MENU_AE_SAN_ANTONIO_49x67_5cm { get; set; }
            //public bool MEDIDA_ESPECIAL_AE_TECAMAC_48x67_5cm { get; set; }
            //public bool MEDIDA_ESPECIAL_AE_VILLA_GARCIA_45x65cm { get; set; }
            //public bool MEDIDA_ESPECIAL_AE_XOLA_49_9x66_9cm { get; set; }
            //public bool MEDIDA_ESPECIAL_AE_ZUAZUA_51x71cm { get; set; }
            //public bool MEDIDA_ESPECIAL_AE_VALLE_SOLEADO_51x71cm { get; set; }
            //public bool MEDIDA_ESPECIAL_AE_MIRASIERRA_46x68cm { get; set; }
            //public bool MEDIDA_ESPECIAL_AE_CELAYA_50x68_5cm { get; set; }
            //public bool MEDIDA_ESPECIAL_AE_CANDILES_49_5x73_5cm { get; set; }
            //public bool MEDIDA_ESPECIAL_60_8x85cm { get; set; }
            //public bool MEDIDA_ESPECIAL_CORREO_MAYOR { get; set; }
            //public bool MEDIDA_ESPECIAL_MALL_ORIENTE_100x120cm { get; set; }
            //public bool MEDIDA_ESPECIAL_ZARAGOZA_90x100cm_60x90cm { get; set; }
            //public bool MEDIDA_ESPECIAL_ZUAZUA_87x120cm { get; set; }
            //public bool MEDIDA_BACKLIGHT_55_5X75_5CM { get; set; }
            //public bool MEDIDA_BACKLIGHT_59_5X79CM { get; set; }
            //public bool MEDIDAS_ESPECIALES_MENU { get; set; }

            ////00000000000000000000000000 POR EQUIPO EN EL RESTAURANTE 0000000000000000000000000000000000

            //public string TipoDeCaja { get; set; }
            //public string AcomodoDeCajas { get; set; }
            //public string NoMesaDeAreaComedor { get; set; }
            //public string NoMesaDeAreaDeJuegos { get; set; }
            //public string NumeroDeVentanas { get; set; }
            //public string UbicacionPantallas { get; set; }

        }

        public class MaterialesTiendasCampaña
        {
            public int ArticuloKFCId { get; set; }
            public string ArticuloKFC { get; set; }
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
            public string TipoTienda { get; set; }
            //public string Tipo { get; set; }
            //public string NuevoNivelDePrecio { get; set; }
            //public bool MenuDigital { get; set; }
            //public string CantidadDePantallas { get; set; }
            //public bool TerceraReceta { get; set; }
            //public bool Arroz { get; set; }
            //public bool Hamburgesas { get; set; }
            //public bool Ensalada { get; set; }
            //public bool PET2Litros { get; set; }
            //public bool Postres { get; set; }
            //public bool BisquetMiel { get; set; }
            //public bool KeCono { get; set; }
            //public bool KREAMBALL { get; set; }
            //public bool MenuBackLigth { get; set; }
            //public bool Autoexpress { get; set; }
            //public bool CopeteAERemodelado { get; set; }
            //public bool CopeteAETradicional { get; set; }
            //public bool PanelDeInnovacion { get; set; }
            //public bool DisplayDeBurbuja { get; set; }
            //public bool Delivery { get; set; }
            //public bool MERCADO_DE_PRUEBA { get; set; }
            //public bool AreaDeJuegos { get; set; }
            //public bool COPETE_ESPECIAL_SOPORTE_LATERAL_4_VASOS { get; set; }
            //public bool COPETE_ESPECIAL_SOPORTE_LATERAL_PET_2L { get; set; }
            //public bool DisplayDePiso { get; set; }
            //public bool WCNACIONAL67X100cm { get; set; }
            //public bool AEMedidaEspecial { get; set; }
            //public bool AEHolding { get; set; }
            //public bool AECaribe { get; set; }
            //public bool PanelComplementosHolding { get; set; }
            //public bool PanelDeComplementosSinArrozSin3raReceta { get; set; }
            //public bool PanelALaCartaCaribe { get; set; }
            //public bool PanelALaCartaCaribeSin3raReceta { get; set; }
            //public bool PanelALaCartaHolding { get; set; }
            //public bool PanelALaCartaHoldingSin3raReceta { get; set; }
            //public bool PanelDeComplementosDigital { get; set; }
            //public bool PanelComplementosHoldingMR { get; set; }
            //public bool Telefono { get; set; }
            ////00000000000000000000000000 MEDIDAS ESPECIALES 0000000000000000000000000000000000
            //public bool WCMedidaEspecial60_8x85cm { get; set; }
            //public bool WC_MEDIDA_ESPECIAL_MALL_ORIENTE_100x120cm { get; set; }
            //public bool WC_MEDIDA_ESPECIAL_ZUAZUA_87x120cm { get; set; }
            //public bool WC_MEDIDA_ESPECIAL_CORREO_MAYOR_60x90cm { get; set; }
            //public bool WC_MEDIDA_ESPECIAL_ZARAGOZA_90x100cm { get; set; }
            //public bool MedidaEspecialPanelDeComplementos { get; set; }
            //public bool MEDIDA_ESPECIAL_PRE_MENU_AE_SAN_ANTONIO_49x67_5cm { get; set; }
            //public bool MEDIDA_ESPECIAL_AE_TECAMAC_48x67_5cm { get; set; }
            //public bool MEDIDA_ESPECIAL_AE_VILLA_GARCIA_45x65cm { get; set; }
            //public bool MEDIDA_ESPECIAL_AE_XOLA_49_9x66_9cm { get; set; }
            //public bool MEDIDA_ESPECIAL_AE_ZUAZUA_51x71cm { get; set; }
            //public bool MEDIDA_ESPECIAL_AE_VALLE_SOLEADO_51x71cm { get; set; }
            //public bool MEDIDA_ESPECIAL_AE_MIRASIERRA_46x68cm { get; set; }
            //public bool MEDIDA_ESPECIAL_AE_CELAYA_50x68_5cm { get; set; }
            //public bool MEDIDA_ESPECIAL_AE_CANDILES_49_5x73_5cm { get; set; }
            //public bool MEDIDA_ESPECIAL_60_8x85cm { get; set; }
            //public bool MEDIDA_ESPECIAL_CORREO_MAYOR { get; set; }
            //public bool MEDIDA_ESPECIAL_MALL_ORIENTE_100x120cm { get; set; }
            //public bool MEDIDA_ESPECIAL_ZARAGOZA_90x100cm_60x90cm { get; set; }
            //public bool MEDIDA_ESPECIAL_ZUAZUA_87x120cm { get; set; }
            //public bool MEDIDA_BACKLIGHT_55_5X75_5CM { get; set; }
            //public bool MEDIDA_BACKLIGHT_59_5X79CM { get; set; }
            //public bool MEDIDAS_ESPECIALES_MENU { get; set; }

            //public string TipoDeCaja { get; set; }
            //public string AcomodoDeCajas { get; set; }
            //public string NoMesaDeAreaComedor { get; set; }
            //public string NoMesaDeAreaDeJuegos { get; set; }
            //public string NumeroDeVentanas { get; set; }
            //public string UbicacionPantallas { get; set; }
            public int ProveedorId { get; set; }
            public int FamiliaId { get; set; }
        }

        public class CodigosMaterialesTienda
        {
            public string ArticuloKFC { get; set; }
            public string Campaña { get; set; }
            public string Proveedor { get; set; }
            [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
            public double TotalCantidad { get; set; }
            public int CampañaId { get; set; }
            public int Codigo { get; set; }

        }

        public class CodigosMaterialesImagenes
        {
            public string ArticuloKFC { get; set; }
            public string LigaImagen { get; set; }
            public string Imagen { get; set; }
        }

        public class CodigosMaterialesEQFQSTK
        {
            public string ArticuloKFC { get; set; }
            [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
            public double TotalCantidad { get; set; }
            public string EquityFranquicia { get; set; }
        }

        public class ProveedoresTotal
        {
            public string Proveedor { get; set; }
            [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
            public double TotalCantidad { get; set; }
            public int CampañaId { get; set; }
        }

        public class MaterialTotal
        {
            public int ArticuloKFCId { get; set; }
            public string ArticuloKFC { get; set; }

            [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
            public double TotalCantidad { get; set; }
            public int ProveedorId { get; set; }

        }

        public class TiendaTotal
        {
            public string Restaurante { get; set; }

            [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
            public double TotalCantidad { get; set; }

        }

        // GET: Campañas
        [AuthorizeUser(idOperacion: 5)]
        public ActionResult Index()
        {
            Session["iconoTitulo"] = "fas fa-calendar-alt";
            Session["homeB"] = string.Empty;
            Session["rolesB"] = string.Empty;
            Session["compañiasB"] = string.Empty;
            Session["usuariosB"] = string.Empty;
            Session["regionesB"] = string.Empty;
            Session["ciudadesB"] = string.Empty;
            Session["restaurantesB"] = string.Empty;
            Session["familiasB"] = string.Empty;
            Session["materialesB"] = string.Empty;
            Session["campañasB"] = "active";
            Session["reglasB"] = string.Empty;
            Session["bitacoraB"] = string.Empty;

            return View();
        }

        [AuthorizeUser(idOperacion: 5)]
        public ActionResult ArtKFC(int id, string prov)
        {
            MemoryStream ms = new MemoryStream();

            Document document = new Document(PageSize.LETTER, 25f, 25f, 60f, 50f);
            PdfWriter pw = PdfWriter.GetInstance(document, ms);

            string pathImage = Server.MapPath("~/Content/images/starbucks_logo.png");
            string pathImageMW = Server.MapPath("~/Content/images/starbucks_logo.png");

            pw.PageEvent = new HeaderFooter(pathImage);
            //pw.PageEvent = new HeaderFooter(pathImage, pathImageMW);

            document.Open();

            string heroFont = Server.MapPath("~/Content/Fonts/Hero.otf");

            BaseFont font = BaseFont.CreateFont(heroFont, BaseFont.CP1250, BaseFont.EMBEDDED);
            Font fontBlack = new Font(font, 10, 0, BaseColor.BLACK);
            Font fontRed = new Font(font, 10, 0, BaseColor.RED);
            Font fontRed2 = new Font(font, 14, 1, BaseColor.RED);

            PdfPTable tabla = new PdfPTable(7);
            tabla.WidthPercentage = 100f;

            var codigosMateriales = db.Database.SqlQuery<CodigosMaterialesTotal>("spGetMaterialesCodigosCampaña @CampañaId",
                new SqlParameter("@CampañaId", id)).ToList();

            var materialesProv = codigosMateriales.Where(x => x.Proveedor == prov).ToList();

            var familias = codigosMateriales.GroupBy(x => x.Familia).ToList();

            foreach (var familia in familias)
            {
                var materialesFam = materialesProv.Where(x => x.Familia == familia.Key).ToList();

                PdfPCell pCell = new PdfPCell();

                Chunk linea = new Chunk(new LineSeparator(2f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, 0f));

                if (materialesFam.Count != 0)
                {
                    pCell = new PdfPCell(new Paragraph(familia.Key.ToUpper(), fontRed2));
                    pCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pCell.Colspan = 7;
                    pCell.Border = 0;
                    tabla.AddCell(pCell);

                    pCell = new PdfPCell(new Paragraph(linea));
                    pCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pCell.Colspan = 7;
                    pCell.Border = 0;
                    tabla.AddCell(pCell);
                }



                foreach (var codigo in materialesFam)
                {

                    BarcodeDatamatrix datamatrix = new BarcodeDatamatrix();
                    //datamatrix.Generate(codigo.Codigo.ToString() + " / " + codigo.ArticuloKFC);
                    datamatrix.Generate(codigo.Codigo.ToString());
                    Image image = datamatrix.CreateImage();
                    image.ScaleAbsolute(40f, 40f);
                    image.Border = 0;
                    //document.Add(image);

                    pCell = new PdfPCell(image, false);
                    pCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pCell.Border = 0;
                    pCell.PaddingTop = 5f;
                    tabla.AddCell(pCell);

                    pCell = new PdfPCell(new Paragraph(codigo.Codigo.ToString(), fontBlack));
                    pCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pCell.Border = 0;
                    tabla.AddCell(pCell);

                    pCell = new PdfPCell(new Paragraph(codigo.ArticuloKFC.ToUpper(), fontRed));
                    pCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pCell.Colspan = 5;
                    pCell.Border = 0;
                    tabla.AddCell(pCell);

                    linea = new Chunk(new LineSeparator(2f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, 0f));

                    pCell = new PdfPCell(new Paragraph(linea));
                    pCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pCell.Colspan = 7;
                    pCell.Border = 0;
                    tabla.AddCell(pCell);

                    //document.Add(linea);
                }
            }


            document.Add(tabla);
            document.Close();

            byte[] bytesStream = ms.ToArray();
            ms = new MemoryStream();
            ms.Write(bytesStream, 0, bytesStream.Length);
            ms.Position = 0;

            return new FileStreamResult(ms, "application/pdf");
        }

        public ActionResult GetData()
        {
            var campañasList = db.Database.SqlQuery<Campaña>("spGetCampañas").ToList();

            return Json(new { data = campañasList }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                var añoActual = DateTime.Now.Year.ToString();
                var año = añoActual.Substring(2, 2);
                var camp = db.Campañas.OrderByDescending(x => x.Nombre).FirstOrDefault();
                var numCampaña = 0;
                var consecutivo = string.Empty;
                if (camp != null)
                {
                    int digitos = camp.Nombre.Length;
                    numCampaña = Convert.ToInt32(camp.Nombre.Substring((digitos - 2), 2)) + 1;
                    consecutivo = "0" + numCampaña.ToString();
                }
                else
                {
                    consecutivo = "01";
                }

                var nuevaCampaña = año + consecutivo;

                return PartialView(new Campaña { Nombre = nuevaCampaña });
            }
            else
            {
                //ViewBag.Consecutivo = db.Campañas.Where(x => x.CampañaId == id).FirstOrDefault().Nombre;
                return PartialView(db.Campañas.Where(x => x.CampañaId == id).FirstOrDefault());
            }
        }

        [AuthorizeUser(idOperacion: 1)]
        [HttpPost]
        public ActionResult AddOrEdit(Campaña campaña)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            if (campaña.CampañaId == 0)
            {
                campaña.Generada = "NO";
                db.Campañas.Add(campaña);
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    MovementsHelper.AgregarArticulosNuevaCampaña(campaña.CampañaId);

                    movimiento = "Agregar Campaña " + campaña.CampañaId + " " + campaña.Nombre + " / " + campaña.Descripcion;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    return Json(new { success = true, message = "CAMPAÑA AGREGADA" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                db.Entry(campaña).State = EntityState.Modified;
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {

                    movimiento = "Actualizar Campaña " + campaña.CampañaId + " " + campaña.Nombre + " / " + campaña.Descripcion;
                    MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                    return Json(new { success = true, message = "CAMPAÑA ACTUALIZADA" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        // GET: Campañas/Details/5
        [AuthorizeUser(idOperacion: 4)]
        public ActionResult CreateCampArt(int? id, string cat)
        {
            Session["iconoTitulo"] = "fas fa-calendar-alt";
            Session["homeB"] = string.Empty;
            Session["franquiciasB"] = string.Empty;
            Session["equityB"] = "active";
            Session["stockB"] = string.Empty;
            Session["campañasB"] = string.Empty;

            //var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault();

            //if (usuario == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            var tipoTienda = string.Empty;

            tipoTienda = cat;

            Session["TipoRestaurante"] = tipoTienda;

            var campaña = db.Campañas.Where(x => x.CampañaId == id).FirstOrDefault();

            ViewBag.Campañas = campaña.Generada;

            var campañaId = id;

            var materialesCampaña = db.Database.SqlQuery<MaterialesTiendasCampaña>("spGetMaterialesCampaña @Categoria, @CampañaId",
                    new SqlParameter("@Categoria", tipoTienda),
                    new SqlParameter("@CampañaId", campañaId)).ToList();

            //var articulosTMP = db.Database.SqlQuery<MaterialesCampaña>("spGetCampañaArticuloTMPs @CampañaId",
            //        new SqlParameter("@CampañaId", campañaId)).ToList();

            //var articulosTMP = db.CampañaArticuloTMPs
            //                       .Where(x => x.CampañaId == campañaId)
            //                       .GroupBy(x => new
            //                       {
            //                           x.ArticuloKFCId,
            //                           x.ArticuloKFC.Descripcion,
            //                           x.CampañaId,
            //                           x.Cantidad,
            //                           x.TiendaId,
            //                           x.Habilitado,
            //                           x.ArticuloKFC.ProveedorId,
            //                           x.ArticuloKFC.FamiliaId,
            //                       })
            //                       .Select(x => new MaterialesCampaña()
            //                       {
            //                           ArticuloKFCId = x.Key.ArticuloKFCId,
            //                           Campaña = campaña.Nombre + " / " + campaña.Descripcion,
            //                           CampañaId = x.Key.CampañaId,
            //                           ArticuloKFC = x.Key.Descripcion,
            //                           Cantidad = x.Key.Cantidad,
            //                           TiendaId = x.Key.TiendaId,
            //                           Habilitado = x.Key.Habilitado,
            //                           ProveedorId = x.Key.ProveedorId,
            //                           FamiliaId = x.Key.FamiliaId,
            //                       }).ToList();

            //var misMateriales = articulosTMP.Where(x => x.Habilitado == true).ToList();

            //var tiendasCampaña = db.Database.SqlQuery<TiendasCampaña>("spGetTiendasCampañas @Categoria",
            //        new SqlParameter("@Categoria", tipoTienda)).ToList();

            //var tiendasCampaña = db.Tiendas
            //                .Where(x => x.EquityFranquicia == tipoTienda)
            //                .GroupBy(x => new
            //                {
            //                    x.Restaurante,
            //                    x.CCoFranquicia,
            //                    x.TiendaId,
            //                    x.TipoTienda.Tipo,
            //                    x.NivelPrecio.Descripcion,
            //                    x.MenuDigital,
            //                    x.CantidadDePantallas,
            //                    x.Clasificacion,
            //                    x.Region.Nombre,
            //                    NombreCiudad = x.Ciudad.Nombre,
            //                    x.Direccion,
            //                    x.TerceraReceta,
            //                    x.Arroz,
            //                    x.Hamburgesas,
            //                    x.Ensalada,
            //                    x.PET2Litros,
            //                    x.Postres,
            //                    x.BisquetMiel,
            //                    x.KeCono,
            //                    x.KREAMBALL,
            //                    x.MenuBackLigth,
            //                    x.Autoexpress,
            //                    x.CopeteAERemodelado,
            //                    x.CopeteAETradicional,
            //                    x.PanelDeInnovacion,
            //                    x.DisplayDeBurbuja,
            //                    x.Delivery,
            //                    x.MERCADO_DE_PRUEBA,
            //                    x.AreaDeJuegos,
            //                    x.COPETE_ESPECIAL_SOPORTE_LATERAL_4_VASOS,
            //                    x.COPETE_ESPECIAL_SOPORTE_LATERAL_PET_2L,
            //                    x.DisplayDePiso,
            //                    x.WCNACIONAL67X100cm,
            //                    x.AECaribe,
            //                    x.AEHolding,
            //                    x.AEMedidaEspecial,
            //                    x.PanelALaCartaCaribe,
            //                    x.PanelALaCartaCaribeSin3raReceta,
            //                    x.PanelALaCartaHolding,
            //                    x.PanelALaCartaHoldingSin3raReceta,
            //                    x.PanelComplementosHolding,
            //                    x.PanelDeComplementosSinArrozSin3raReceta,
            //                    x.PanelDeComplementosDigital,
            //                    x.PanelComplementosHoldingMR,
            //                    x.Telefono,
            //                    x.UbicacionPantallas,
            //                    TipoDeCajaNombre = x.TipoDeCaja.Descripcion,
            //                    x.AcomodoDeCajas,
            //                    x.NoMesaDeAreaComedor,
            //                    x.NoMesaDeAreaDeJuegos,
            //                    x.NumeroDeVentanas,
            //                    x.Observaciones,
            //                    x.WCMedidaEspecial60_8x85cm,
            //                    x.WC_MEDIDA_ESPECIAL_MALL_ORIENTE_100x120cm,
            //                    x.WC_MEDIDA_ESPECIAL_ZUAZUA_87x120cm,
            //                    x.WC_MEDIDA_ESPECIAL_CORREO_MAYOR_60x90cm,
            //                    x.WC_MEDIDA_ESPECIAL_ZARAGOZA_90x100cm,
            //                    x.MedidaEspecialPanelDeComplementos,
            //                    x.MEDIDA_ESPECIAL_PRE_MENU_AE_SAN_ANTONIO_49x67_5cm,
            //                    x.MEDIDA_ESPECIAL_AE_TECAMAC_48x67_5cm,
            //                    x.MEDIDA_ESPECIAL_AE_VILLA_GARCIA_45x65cm,
            //                    x.MEDIDA_ESPECIAL_AE_XOLA_49_9x66_9cm,
            //                    x.MEDIDA_ESPECIAL_AE_ZUAZUA_51x71cm,
            //                    x.MEDIDA_ESPECIAL_AE_VALLE_SOLEADO_51x71cm,
            //                    x.MEDIDA_ESPECIAL_AE_MIRASIERRA_46x68cm,
            //                    x.MEDIDA_ESPECIAL_AE_CELAYA_50x68_5cm,
            //                    x.MEDIDA_ESPECIAL_AE_CANDILES_49_5x73_5cm,
            //                    x.MEDIDA_BACKLIGHT_55_5X75_5CM,
            //                    x.MEDIDA_BACKLIGHT_59_5X79CM,
            //                    x.MEDIDAS_ESPECIALES_MENU,
            //                })
            //                .Select(x => new TiendasCampaña()
            //                {
            //                    Restaurante = x.Key.Restaurante,
            //                    TiendaId = x.Key.TiendaId,
            //                    Clasificacion = x.Key.Clasificacion,
            //                    Region = x.Key.Nombre,
            //                    Ciudad = x.Key.NombreCiudad,
            //                    Direccion = x.Key.Direccion,
            //                    CC = x.Key.CCoFranquicia,
            //                    TipoTienda = tipoTienda,
            //                    CantidadDePantallas = x.Key.CantidadDePantallas,
            //                    MenuDigital = x.Key.MenuDigital,
            //                    Tipo = x.Key.Tipo,
            //                    NuevoNivelDePrecio = x.Key.Descripcion,
            //                    TerceraReceta = x.Key.TerceraReceta,
            //                    Arroz = x.Key.Arroz,
            //                    Hamburgesas = x.Key.Hamburgesas,
            //                    Ensalada = x.Key.Ensalada,
            //                    PET2Litros = x.Key.PET2Litros,
            //                    Postres = x.Key.Postres,
            //                    BisquetMiel = x.Key.BisquetMiel,
            //                    KeCono = x.Key.KeCono,
            //                    KREAMBALL = x.Key.KREAMBALL,
            //                    AcomodoDeCajas = x.Key.AcomodoDeCajas,
            //                    AreaDeJuegos = x.Key.AreaDeJuegos,
            //                    Autoexpress = x.Key.Autoexpress,
            //                    CopeteAERemodelado = x.Key.CopeteAERemodelado,
            //                    CopeteAETradicional = x.Key.CopeteAETradicional,
            //                    NumeroDeVentanas = x.Key.NumeroDeVentanas,
            //                    NoMesaDeAreaDeJuegos = x.Key.NoMesaDeAreaDeJuegos,
            //                    COPETE_ESPECIAL_SOPORTE_LATERAL_4_VASOS = x.Key.COPETE_ESPECIAL_SOPORTE_LATERAL_4_VASOS,
            //                    COPETE_ESPECIAL_SOPORTE_LATERAL_PET_2L = x.Key.COPETE_ESPECIAL_SOPORTE_LATERAL_PET_2L,
            //                    Delivery = x.Key.Delivery,
            //                    DisplayDeBurbuja = x.Key.DisplayDeBurbuja,
            //                    DisplayDePiso = x.Key.DisplayDePiso,
            //                    MenuBackLigth = x.Key.MenuBackLigth,
            //                    MERCADO_DE_PRUEBA = x.Key.MERCADO_DE_PRUEBA,
            //                    NoMesaDeAreaComedor = x.Key.NoMesaDeAreaComedor,
            //                    PanelDeInnovacion = x.Key.PanelDeInnovacion,
            //                    TipoDeCaja = x.Key.TipoDeCajaNombre,
            //                    WCNACIONAL67X100cm = x.Key.WCNACIONAL67X100cm,
            //                    AECaribe = x.Key.AECaribe,
            //                    AEHolding = x.Key.AEHolding,
            //                    AEMedidaEspecial = x.Key.AEMedidaEspecial,
            //                    PanelALaCartaCaribe = x.Key.PanelALaCartaCaribe,
            //                    PanelALaCartaCaribeSin3raReceta = x.Key.PanelALaCartaCaribeSin3raReceta,
            //                    PanelALaCartaHolding = x.Key.PanelALaCartaHolding,
            //                    PanelALaCartaHoldingSin3raReceta = x.Key.PanelALaCartaHoldingSin3raReceta,
            //                    PanelComplementosHolding = x.Key.PanelComplementosHolding,
            //                    PanelDeComplementosSinArrozSin3raReceta = x.Key.PanelDeComplementosSinArrozSin3raReceta,
            //                    PanelComplementosHoldingMR = x.Key.PanelComplementosHoldingMR,
            //                    PanelDeComplementosDigital = x.Key.PanelDeComplementosDigital,
            //                    Telefono = x.Key.Telefono,
            //                    UbicacionPantallas = x.Key.UbicacionPantallas,
            //                    WCMedidaEspecial60_8x85cm = x.Key.WCMedidaEspecial60_8x85cm,
            //                    WC_MEDIDA_ESPECIAL_MALL_ORIENTE_100x120cm = x.Key.WC_MEDIDA_ESPECIAL_MALL_ORIENTE_100x120cm,
            //                    WC_MEDIDA_ESPECIAL_ZUAZUA_87x120cm = x.Key.WC_MEDIDA_ESPECIAL_ZUAZUA_87x120cm,
            //                    WC_MEDIDA_ESPECIAL_CORREO_MAYOR_60x90cm = x.Key.WC_MEDIDA_ESPECIAL_CORREO_MAYOR_60x90cm,
            //                    WC_MEDIDA_ESPECIAL_ZARAGOZA_90x100cm = x.Key.WC_MEDIDA_ESPECIAL_ZARAGOZA_90x100cm,
            //                    MedidaEspecialPanelDeComplementos = x.Key.MedidaEspecialPanelDeComplementos,
            //                    MEDIDA_ESPECIAL_PRE_MENU_AE_SAN_ANTONIO_49x67_5cm = x.Key.MEDIDA_ESPECIAL_PRE_MENU_AE_SAN_ANTONIO_49x67_5cm,
            //                    MEDIDA_ESPECIAL_AE_TECAMAC_48x67_5cm = x.Key.MEDIDA_ESPECIAL_AE_TECAMAC_48x67_5cm,
            //                    MEDIDA_ESPECIAL_AE_VILLA_GARCIA_45x65cm = x.Key.MEDIDA_ESPECIAL_AE_VILLA_GARCIA_45x65cm,
            //                    MEDIDA_ESPECIAL_AE_XOLA_49_9x66_9cm = x.Key.MEDIDA_ESPECIAL_AE_XOLA_49_9x66_9cm,
            //                    MEDIDA_ESPECIAL_AE_ZUAZUA_51x71cm = x.Key.MEDIDA_ESPECIAL_AE_ZUAZUA_51x71cm,
            //                    MEDIDA_ESPECIAL_AE_VALLE_SOLEADO_51x71cm = x.Key.MEDIDA_ESPECIAL_AE_VALLE_SOLEADO_51x71cm,
            //                    MEDIDA_ESPECIAL_AE_MIRASIERRA_46x68cm = x.Key.MEDIDA_ESPECIAL_AE_MIRASIERRA_46x68cm,
            //                    MEDIDA_ESPECIAL_AE_CELAYA_50x68_5cm = x.Key.MEDIDA_ESPECIAL_AE_CELAYA_50x68_5cm,
            //                    MEDIDA_ESPECIAL_AE_CANDILES_49_5x73_5cm = x.Key.MEDIDA_ESPECIAL_AE_CANDILES_49_5x73_5cm,
            //                    //MEDIDA_ESPECIAL_60_8x85cm = x.Key.MEDIDA_ESPECIAL_60_8x85cm,
            //                    //MEDIDA_ESPECIAL_CORREO_MAYOR = x.Key.MEDIDA_ESPECIAL_CORREO_MAYOR,
            //                    //MEDIDA_ESPECIAL_MALL_ORIENTE_100x120cm = x.Key.MEDIDA_ESPECIAL_MALL_ORIENTE_100x120cm,
            //                    //MEDIDA_ESPECIAL_ZARAGOZA_90x100cm_60x90cm = x.Key.MEDIDA_ESPECIAL_ZARAGOZA_90x100cm_60x90cm,
            //                    //MEDIDA_ESPECIAL_ZUAZUA_87x120cm = x.Key.MEDIDA_ESPECIAL_ZUAZUA_87x120cm,
            //                    MEDIDA_BACKLIGHT_55_5X75_5CM = x.Key.MEDIDA_BACKLIGHT_55_5X75_5CM,
            //                    MEDIDA_BACKLIGHT_59_5X79CM = x.Key.MEDIDA_BACKLIGHT_59_5X79CM,
            //                    MEDIDAS_ESPECIALES_MENU = x.Key.MEDIDAS_ESPECIALES_MENU,
            //                }).ToList();

            //var misTiendas = tiendasCampaña.ToList();

            //var materialesCampaña = articulosTMP.Join(tiendasCampaña,
            //                     artCamp => artCamp.TiendaId,
            //                     tienCamp => tienCamp.TiendaId,
            //                     (artCamp, tienCamp) => new { tiendas = tienCamp, materiales = artCamp })
            //                .Where(x => x.tiendas.TiendaId == x.materiales.TiendaId)
            //                .Where(x => x.tiendas.TipoTienda == tipoTienda)
            //                .GroupBy(x => new
            //                {
            //                    x.tiendas.Restaurante,
            //                    x.tiendas.Clasificacion,
            //                    x.tiendas.CC,
            //                    x.tiendas.Region,
            //                    x.tiendas.Ciudad,
            //                    x.tiendas.Direccion,
            //                    x.tiendas.TiendaId,
            //                    x.tiendas.TipoTienda,
            //                    //x.tiendas.NuevoNivelDePrecio,
            //                    //x.tiendas.MenuDigital,
            //                    //x.tiendas.Tipo,
            //                    //x.tiendas.CantidadDePantallas,
            //                    //x.tiendas.TerceraReceta,
            //                    //x.tiendas.Arroz,
            //                    //x.tiendas.Hamburgesas,
            //                    //x.tiendas.Ensalada,
            //                    //x.tiendas.PET2Litros,
            //                    //x.tiendas.Postres,
            //                    //x.tiendas.BisquetMiel,
            //                    //x.tiendas.KeCono,
            //                    //x.tiendas.KREAMBALL,
            //                    //x.tiendas.AcomodoDeCajas,
            //                    //x.tiendas.AreaDeJuegos,
            //                    //x.tiendas.Autoexpress,
            //                    //x.tiendas.CopeteAERemodelado,
            //                    //x.tiendas.CopeteAETradicional,
            //                    //x.tiendas.COPETE_ESPECIAL_SOPORTE_LATERAL_4_VASOS,
            //                    //x.tiendas.COPETE_ESPECIAL_SOPORTE_LATERAL_PET_2L,
            //                    //x.tiendas.Delivery,
            //                    //x.tiendas.DisplayDeBurbuja,
            //                    //x.tiendas.DisplayDePiso,
            //                    //x.tiendas.MenuBackLigth,
            //                    //x.tiendas.MERCADO_DE_PRUEBA,
            //                    //x.tiendas.NoMesaDeAreaComedor,
            //                    //x.tiendas.NoMesaDeAreaDeJuegos,
            //                    //x.tiendas.NumeroDeVentanas,
            //                    //x.tiendas.PanelDeInnovacion,
            //                    //x.tiendas.TipoDeCaja,
            //                    //x.tiendas.WCNACIONAL67X100cm,
            //                    //x.tiendas.AECaribe,
            //                    //x.tiendas.AEHolding,
            //                    //x.tiendas.AEMedidaEspecial,
            //                    //x.tiendas.PanelALaCartaCaribe,
            //                    //x.tiendas.PanelALaCartaCaribeSin3raReceta,
            //                    //x.tiendas.PanelALaCartaHolding,
            //                    //x.tiendas.PanelALaCartaHoldingSin3raReceta,
            //                    //x.tiendas.PanelComplementosHolding,
            //                    //x.tiendas.PanelDeComplementosSinArrozSin3raReceta,
            //                    //x.tiendas.PanelComplementosHoldingMR,
            //                    //x.tiendas.PanelDeComplementosDigital,
            //                    //x.tiendas.Telefono,
            //                    //x.tiendas.UbicacionPantallas,
            //                    //x.tiendas.WCMedidaEspecial60_8x85cm,
            //                    //x.tiendas.WC_MEDIDA_ESPECIAL_MALL_ORIENTE_100x120cm,
            //                    //x.tiendas.WC_MEDIDA_ESPECIAL_ZUAZUA_87x120cm,
            //                    //x.tiendas.WC_MEDIDA_ESPECIAL_CORREO_MAYOR_60x90cm,
            //                    //x.tiendas.WC_MEDIDA_ESPECIAL_ZARAGOZA_90x100cm,
            //                    //x.tiendas.MedidaEspecialPanelDeComplementos,
            //                    //x.tiendas.MEDIDA_ESPECIAL_PRE_MENU_AE_SAN_ANTONIO_49x67_5cm,
            //                    //x.tiendas.MEDIDA_ESPECIAL_AE_TECAMAC_48x67_5cm,
            //                    //x.tiendas.MEDIDA_ESPECIAL_AE_VILLA_GARCIA_45x65cm,
            //                    //x.tiendas.MEDIDA_ESPECIAL_AE_XOLA_49_9x66_9cm,
            //                    //x.tiendas.MEDIDA_ESPECIAL_AE_ZUAZUA_51x71cm,
            //                    //x.tiendas.MEDIDA_ESPECIAL_AE_VALLE_SOLEADO_51x71cm,
            //                    //x.tiendas.MEDIDA_ESPECIAL_AE_MIRASIERRA_46x68cm,
            //                    //x.tiendas.MEDIDA_ESPECIAL_AE_CELAYA_50x68_5cm,
            //                    //x.tiendas.MEDIDA_ESPECIAL_AE_CANDILES_49_5x73_5cm,
            //                    //x.tiendas.MEDIDA_ESPECIAL_60_8x85cm,
            //                    //x.tiendas.MEDIDA_ESPECIAL_CORREO_MAYOR,
            //                    //x.tiendas.MEDIDA_ESPECIAL_MALL_ORIENTE_100x120cm,
            //                    //x.tiendas.MEDIDA_ESPECIAL_ZARAGOZA_90x100cm_60x90cm,
            //                    //x.tiendas.MEDIDA_ESPECIAL_ZUAZUA_87x120cm,
            //                    //x.tiendas.MEDIDA_BACKLIGHT_55_5X75_5CM,
            //                    //x.tiendas.MEDIDA_BACKLIGHT_59_5X79CM,
            //                    //x.tiendas.MEDIDAS_ESPECIALES_MENU,
            //                    x.materiales.ArticuloKFCId,
            //                    x.materiales.ArticuloKFC,
            //                    x.materiales.Cantidad,
            //                    x.materiales.Campaña,
            //                    x.materiales.CampañaId,
            //                    x.materiales.Habilitado,
            //                    x.materiales.ProveedorId,
            //                    x.materiales.FamiliaId,
            //                })
            //                .Select(x => new MaterialesTiendasCampaña()
            //                {
            //                    ArticuloKFCId = x.Key.ArticuloKFCId,
            //                    ArticuloKFC = x.Key.ArticuloKFC,
            //                    Campaña = x.Key.Campaña,
            //                    CampañaId = x.Key.CampañaId,
            //                    Cantidad = x.Key.Cantidad,
            //                    CC = x.Key.CC,
            //                    Clasificacion = x.Key.Clasificacion,
            //                    Restaurante = x.Key.Restaurante,
            //                    Ciudad = x.Key.Ciudad,
            //                    Direccion = x.Key.Direccion,
            //                    Region = x.Key.Region,
            //                    TiendaId = x.Key.TiendaId,
            //                    TipoTienda = x.Key.TipoTienda,
            //                    Habilitado = x.Key.Habilitado,
            //                    //CantidadDePantallas = x.Key.CantidadDePantallas,
            //                    //MenuDigital = x.Key.MenuDigital,
            //                    //NuevoNivelDePrecio = x.Key.NuevoNivelDePrecio,
            //                    //Tipo = x.Key.Tipo,
            //                    //TerceraReceta = x.Key.TerceraReceta,
            //                    //Arroz = x.Key.Arroz,
            //                    //Hamburgesas = x.Key.Hamburgesas,
            //                    //Ensalada = x.Key.Ensalada,
            //                    //PET2Litros = x.Key.PET2Litros,
            //                    //Postres = x.Key.Postres,
            //                    //BisquetMiel = x.Key.BisquetMiel,
            //                    //KeCono = x.Key.KeCono,
            //                    //KREAMBALL = x.Key.KREAMBALL,
            //                    //AcomodoDeCajas = x.Key.AcomodoDeCajas,
            //                    //AreaDeJuegos = x.Key.AreaDeJuegos,
            //                    //Autoexpress = x.Key.Autoexpress,
            //                    //CopeteAERemodelado = x.Key.CopeteAERemodelado,
            //                    //CopeteAETradicional = x.Key.CopeteAETradicional,
            //                    //NumeroDeVentanas = x.Key.NumeroDeVentanas,
            //                    //NoMesaDeAreaDeJuegos = x.Key.NoMesaDeAreaDeJuegos,
            //                    //COPETE_ESPECIAL_SOPORTE_LATERAL_4_VASOS = x.Key.COPETE_ESPECIAL_SOPORTE_LATERAL_4_VASOS,
            //                    //COPETE_ESPECIAL_SOPORTE_LATERAL_PET_2L = x.Key.COPETE_ESPECIAL_SOPORTE_LATERAL_PET_2L,
            //                    //Delivery = x.Key.Delivery,
            //                    //DisplayDeBurbuja = x.Key.DisplayDeBurbuja,
            //                    //DisplayDePiso = x.Key.DisplayDePiso,
            //                    //MenuBackLigth = x.Key.MenuBackLigth,
            //                    //MERCADO_DE_PRUEBA = x.Key.MERCADO_DE_PRUEBA,
            //                    //NoMesaDeAreaComedor = x.Key.NoMesaDeAreaComedor,
            //                    //PanelDeInnovacion = x.Key.PanelDeInnovacion,
            //                    //TipoDeCaja = x.Key.TipoDeCaja,
            //                    //WCNACIONAL67X100cm = x.Key.WCNACIONAL67X100cm,
            //                    ProveedorId = x.Key.ProveedorId,
            //                    FamiliaId = x.Key.FamiliaId,
            //                    //AECaribe = x.Key.AECaribe,
            //                    //AEHolding = x.Key.AEHolding,
            //                    //AEMedidaEspecial = x.Key.AEMedidaEspecial,
            //                    //PanelALaCartaCaribe = x.Key.PanelALaCartaCaribe,
            //                    //PanelALaCartaCaribeSin3raReceta = x.Key.PanelALaCartaCaribeSin3raReceta,
            //                    //PanelALaCartaHolding = x.Key.PanelALaCartaHolding,
            //                    //PanelALaCartaHoldingSin3raReceta = x.Key.PanelALaCartaHoldingSin3raReceta,
            //                    //PanelComplementosHolding = x.Key.PanelComplementosHolding,
            //                    //PanelDeComplementosSinArrozSin3raReceta = x.Key.PanelDeComplementosSinArrozSin3raReceta,
            //                    //PanelComplementosHoldingMR = x.Key.PanelComplementosHoldingMR,
            //                    //PanelDeComplementosDigital = x.Key.PanelDeComplementosDigital,
            //                    //Telefono = x.Key.Telefono,
            //                    //UbicacionPantallas = x.Key.UbicacionPantallas,
            //                    //WCMedidaEspecial60_8x85cm = x.Key.WCMedidaEspecial60_8x85cm,
            //                    //WC_MEDIDA_ESPECIAL_MALL_ORIENTE_100x120cm = x.Key.WC_MEDIDA_ESPECIAL_MALL_ORIENTE_100x120cm,
            //                    //WC_MEDIDA_ESPECIAL_ZUAZUA_87x120cm = x.Key.WC_MEDIDA_ESPECIAL_ZUAZUA_87x120cm,
            //                    //WC_MEDIDA_ESPECIAL_CORREO_MAYOR_60x90cm = x.Key.WC_MEDIDA_ESPECIAL_CORREO_MAYOR_60x90cm,
            //                    //WC_MEDIDA_ESPECIAL_ZARAGOZA_90x100cm = x.Key.WC_MEDIDA_ESPECIAL_ZARAGOZA_90x100cm,
            //                    //MedidaEspecialPanelDeComplementos = x.Key.MedidaEspecialPanelDeComplementos,
            //                    //MEDIDA_ESPECIAL_PRE_MENU_AE_SAN_ANTONIO_49x67_5cm = x.Key.MEDIDA_ESPECIAL_PRE_MENU_AE_SAN_ANTONIO_49x67_5cm,
            //                    //MEDIDA_ESPECIAL_AE_TECAMAC_48x67_5cm = x.Key.MEDIDA_ESPECIAL_AE_TECAMAC_48x67_5cm,
            //                    //MEDIDA_ESPECIAL_AE_VILLA_GARCIA_45x65cm = x.Key.MEDIDA_ESPECIAL_AE_VILLA_GARCIA_45x65cm,
            //                    //MEDIDA_ESPECIAL_AE_XOLA_49_9x66_9cm = x.Key.MEDIDA_ESPECIAL_AE_XOLA_49_9x66_9cm,
            //                    //MEDIDA_ESPECIAL_AE_ZUAZUA_51x71cm = x.Key.MEDIDA_ESPECIAL_AE_ZUAZUA_51x71cm,
            //                    //MEDIDA_ESPECIAL_AE_VALLE_SOLEADO_51x71cm = x.Key.MEDIDA_ESPECIAL_AE_VALLE_SOLEADO_51x71cm,
            //                    //MEDIDA_ESPECIAL_AE_MIRASIERRA_46x68cm = x.Key.MEDIDA_ESPECIAL_AE_MIRASIERRA_46x68cm,
            //                    //MEDIDA_ESPECIAL_AE_CELAYA_50x68_5cm = x.Key.MEDIDA_ESPECIAL_AE_CELAYA_50x68_5cm,
            //                    //MEDIDA_ESPECIAL_AE_CANDILES_49_5x73_5cm = x.Key.MEDIDA_ESPECIAL_AE_CANDILES_49_5x73_5cm,
            //                    //MEDIDA_ESPECIAL_60_8x85cm = x.Key.MEDIDA_ESPECIAL_60_8x85cm,
            //                    //MEDIDA_ESPECIAL_CORREO_MAYOR = x.Key.MEDIDA_ESPECIAL_CORREO_MAYOR,
            //                    //MEDIDA_ESPECIAL_MALL_ORIENTE_100x120cm = x.Key.MEDIDA_ESPECIAL_MALL_ORIENTE_100x120cm,
            //                    //MEDIDA_ESPECIAL_ZARAGOZA_90x100cm_60x90cm = x.Key.MEDIDA_ESPECIAL_ZARAGOZA_90x100cm_60x90cm,
            //                    //MEDIDA_ESPECIAL_ZUAZUA_87x120cm = x.Key.MEDIDA_ESPECIAL_ZUAZUA_87x120cm,
            //                    //MEDIDA_BACKLIGHT_55_5X75_5CM = x.Key.MEDIDA_BACKLIGHT_55_5X75_5CM,
            //                    //MEDIDA_BACKLIGHT_59_5X79CM = x.Key.MEDIDA_BACKLIGHT_59_5X79CM,
            //                    //MEDIDAS_ESPECIALES_MENU = x.Key.MEDIDAS_ESPECIALES_MENU,
            //                });

            //var misMaterialesCampaña = materialesCampaña.ToList();

            var totalMaterial = materialesCampaña
                               .Where(x => x.TipoTienda == tipoTienda)
                               .GroupBy(x => new { x.ArticuloKFC, x.ArticuloKFCId, x.ProveedorId })
                               .Select(x => new MaterialTotal()
                               {
                                   ArticuloKFCId = x.Key.ArticuloKFCId,
                                   ArticuloKFC = x.Key.ArticuloKFC,
                                   ProveedorId = x.Key.ProveedorId,
                                   TotalCantidad = x.Sum(t => t.Cantidad),
                               });

            ViewBag.TotalMaterial = totalMaterial.Where(x => x.ProveedorId != 5).OrderBy(x => x.ArticuloKFC).ToList();

            ViewBag.Total = totalMaterial.Sum(x => x.TotalCantidad);

            //var totalRestaurante = materialesCampaña
            //                   .GroupBy(x => new { x.Restaurante })
            //                   .Select(x => new TiendaTotal()
            //                   {
            //                       Restaurante = x.Key.Restaurante,
            //                       TotalCantidad = x.Sum(t => t.Cantidad),
            //                   });

            //ViewBag.TotalRestaurante = totalRestaurante.ToList();

            //var totalMateriales = materialesCampaña.ToList();
            var totalMateriales = materialesCampaña.ToList().OrderBy(p => p.TiendaId).ThenBy(p => p.ArticuloKFC).ToList();

            if (totalMateriales.Count == 0)
            {
                TempData["mensajeLito"] = "Aún no hay cantidades para la Campaña";

                return RedirectToAction("Index");
            }

            return View(totalMateriales.ToList());

        }

        [AuthorizeUser(idOperacion: 3)]
        [HttpPost]
        public ActionResult CloseCampArt(int id)
        {

            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            Campaña campaña = db.Campañas.Where(x => x.CampañaId == id).FirstOrDefault();
            campaña.Generada = "SI";
            db.Entry(campaña).State = EntityState.Modified;
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                var campañas = db.Campañas.Where(x => x.Generada == "NO").ToList();

                if (campañas.Count == 0)
                {
                    db.Database.ExecuteSqlCommand(
                    "spEliminarTodosArticulosTiendas");

                    db.Database.ExecuteSqlCommand(
                    "spDesactivarMateriales");
                }

                movimiento = "Cerrar Campaña " + campaña.CampañaId + " " + campaña.Nombre + " / " + campaña.Descripcion;
                MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                return Json(new { success = true, message = "CAMPAÑA CERRADA" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Campañas/Details/5
        [AuthorizeUser(idOperacion: 7)]
        public ActionResult CodesCampArt(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault();
            if (usuario == null)
            {
                return RedirectToAction("Index");
            }

            var response = MovementsHelper.GenerarCodigos(id);

            if (response.Succeeded)
            {
                var campaña = db.Campañas.Where(x => x.CampañaId == id).FirstOrDefault();

                var codigosMateriales = db.Database.SqlQuery<CodigosMaterialesTotal>("spGetMaterialesCodigosCampaña @CampañaId",
                                            new SqlParameter("@CampañaId", id)).ToList();

                var codigosMaterialesOrdenes = db.Database.SqlQuery<CodigosMaterialesTotalOrdenes>("spGetMaterialesOrdenes @CampañaId",
                                            new SqlParameter("@CampañaId", id)).ToList();

                var vacio = "";

                //var folder = "C:\\";

                var folder = Server.MapPath("~/Content/Archivos/");

                using (StreamWriter streamWriter = new StreamWriter(folder + "Materiales" + campaña.Nombre + ".txt"))
                {
                    foreach (var codigo in codigosMateriales)
                    {
                        var linea = "INSERT INTO Articulos (Codigo, Descripcion, SistemaImpresion,MedExtendida,Sustrato,Tintas,Laminado_FV,Corte,MatPegue,InfAdicional) VALUES ('" + codigo.Codigo + "', '" + codigo.ArticuloKFC.ToUpper() + "', '" + vacio + "', '" + vacio + "', '" + vacio + "', '" + vacio + "', '" + vacio + "', '" + vacio + "', '" + vacio + "', '" + codigo.ArticuloKFC.ToUpper() + "')";
                        streamWriter.WriteLine(linea);
                    }
                }

                using (StreamWriter streamWriter = new StreamWriter(folder + "Materiales" + campaña.Nombre + ".txt", false, Encoding.GetEncoding(1252)))
                {
                    foreach (var codigo in codigosMateriales)
                    {
                        var linea = "INSERT INTO Articulos (Codigo, Descripcion, SistemaImpresion,MedExtendida,Sustrato,Tintas,Laminado_FV,Corte,MatPegue,InfAdicional) VALUES ('" + codigo.Codigo + "', '" + codigo.ArticuloKFC.ToUpper() + "', '" + vacio + "', '" + vacio + "', '" + vacio + "', '" + vacio + "', '" + vacio + "', '" + vacio + "', '" + vacio + "', '" + codigo.ArticuloKFC.ToUpper() + "')";
                        streamWriter.WriteLine(linea);
                    }
                }

                var tiendas = db.Database.SqlQuery<spTiendasActivas>("spGetRestaurantesActivos").ToList();
                var i = 1;

                using (StreamWriter streamWriter = new StreamWriter(folder + "Tiendas" + campaña.Nombre + ".txt", false, Encoding.GetEncoding(1252)))
                {
                    foreach (var tienda in tiendas)
                    {
                        var linea = "INSERT INTO Tiendas (Id, Secuencia, Tienda, Region, Ciudad, IdCampana) VALUES (" + tienda.CCoFranquicia.ToUpper() + ", " + i + ", '" + tienda.CCoFranquicia.ToUpper() + " / " + tienda.Restaurante.ToUpper() + "', '" + tienda.Region.ToUpper() + "', '" + tienda.Ciudad.ToUpper() + "', " + Convert.ToInt32(campaña.Nombre) + ")";
                        streamWriter.WriteLine(linea);

                        i = i + 1;
                    }
                }

                using (StreamWriter streamWriter = new StreamWriter(folder + "Ordenes" + campaña.Nombre + ".txt", false, Encoding.GetEncoding(1252)))
                {
                    foreach (var codigo in codigosMaterialesOrdenes)
                    {
                        var linea = "INSERT INTO Ordenes (CAMPANA, IDTIENDA, IDARTICULO, CANTIDAD) VALUES ('" + campaña.Descripcion.ToUpper() + "', '" + codigo.CCoFranquicia.ToUpper() + "', '" + codigo.Codigo + "', " + codigo.Cantidad + ")";
                        streamWriter.WriteLine(linea);
                    }
                }

                movimiento = "Generar Códigos " + campaña.CampañaId + " " + campaña.Nombre + " / " + campaña.Descripcion;
                MovementsHelper.MovimientosBitacora(usuario.UsuarioId, modulo, movimiento);

                return Json(new { success = true, message = "CODIGOS GENERADOS" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }

            //return RedirectToAction("Index");
            //return RedirectToAction("ResumenProveedor", new { id = id });
        }

        [AuthorizeUser(idOperacion: 3)]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault().UsuarioId;

            db.Database.ExecuteSqlCommand(
            "spEliminarCampaña @CampañaId",
            new SqlParameter("@CampañaId", id));

            db.Database.ExecuteSqlCommand(
            "spEliminarCodigos @CampañaId",
            new SqlParameter("@CampañaId", id));

            Campaña campaña = db.Campañas.Where(x => x.CampañaId == id).FirstOrDefault();
            db.Campañas.Remove(campaña);
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                movimiento = "Eliminar Campaña " + campaña.CampañaId + " " + campaña.Nombre + " / " + campaña.Descripcion;
                MovementsHelper.MovimientosBitacora(usuario, modulo, movimiento);

                return Json(new { success = true, message = "CAMPAÑA ELIMINADA" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeUser(idOperacion: 5)]
        public ActionResult ResumenProveedor(int id)
        {

            var usuario = db.Usuarios.Where(u => u.NombreUsuario == User.Identity.Name).FirstOrDefault();

            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var campaña = db.Campañas.Where(x => x.CampañaId == id).FirstOrDefault();

            var codigosMateriales = db.Database.SqlQuery<CodigosMaterialesTotal>("spGetMaterialesCodigosCampaña @CampañaId",
                new SqlParameter("@CampañaId", id)).ToList();

            if (codigosMateriales.Count == 0)
            {
                Session["Mensaje"] = "AÚN NO SE HAN GENERADO CÓDIGOS";
                return RedirectToAction("Index");
                //return Json(new { success = true, message = "AÚN NO SE HAN GENERADO CÓDIGOS" }, JsonRequestBehavior.AllowGet);
            }

            var materialesTipoTienda = db.Database.SqlQuery<CodigosMaterialesTienda>("spGetMaterialesTipoTienda @CampañaId",
                                        new SqlParameter("@CampañaId", id)).ToList();

            var materialesEqFqStk = db.Database.SqlQuery<CodigosMaterialesEQFQSTK>("spGetMaterialesEqFqStk @CampañaId",
                                        new SqlParameter("@CampañaId", id)).ToList();

            ViewBag.TotalCategoria = materialesEqFqStk.ToList();

            var materialesImagenes = db.Database.SqlQuery<CodigosMaterialesImagenes>("spGetMaterialesImagenes @CampañaId",
                                        new SqlParameter("@CampañaId", id)).ToList();

            ViewBag.Imagenes = materialesImagenes.ToList();

            var totalProv = db.Database.SqlQuery<ProveedoresTotal>("spGetTotalProv @CampañaId",
                            new SqlParameter("@CampañaId", id)).ToList();


            ViewBag.TotalProveedor = totalProv.ToList();

            var totalCantidad = db.Database.SqlQuery<MaterialTotal>("spGetMaterialesTotalResumen @CampañaId",
                new SqlParameter("@CampañaId", id)).ToList();

            ViewBag.TotalMaterial = totalCantidad.ToList();

            var totalMateriales = materialesTipoTienda.ToList().OrderBy(p => p.Proveedor).ThenBy(p => p.ArticuloKFC);

            return View(totalMateriales);
        }

    }

    class HeaderFooter : PdfPageEventHelper
    {
        public HeaderFooter(string logoPath)
        {
            LogoPath = logoPath;
        }

        //public HeaderFooter(string logoPath, string marcaImagen)
        //{
        //    LogoPath = logoPath;
        //    MarcaImagen = marcaImagen;
        //}

        public string LogoPath { get; }
        public string MarcaImagen { get; }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            //PdfContentByte cb = writer.DirectContentUnder;
            //Image image = Image.GetInstance(MarcaImagen);

            //float positionX = (writer.PageSize.Top / 2) - (image.Width / 9);
            //float positionY = (writer.PageSize.Right / 2) - (image.Height / 3);

            //image.SetAbsolutePosition(-370f, -50f);
            //image.ScaleAbsolute(image.PlainWidth / 3 + 100, image.PlainHeight / 3 + 200);
            ////image.ScaleAbsoluteWidth(image.PlainWidth / 3);
            //PdfGState state = new PdfGState();
            //state.FillOpacity = 0.1f;
            //cb.SetGState(state);
            //cb.AddImage(image);

            PdfPTable tbHeader = new PdfPTable(3);
            tbHeader.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            tbHeader.DefaultCell.Border = 0;

            tbHeader.AddCell(new Paragraph());

            PdfPCell pCell = new PdfPCell(new Paragraph("Lista de Materiales"));
            pCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pCell.Border = 0;
            tbHeader.AddCell(pCell);

            tbHeader.AddCell(new Paragraph());

            tbHeader.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetTop(document.TopMargin) + 40, writer.DirectContent);

            PdfPTable tbFooter = new PdfPTable(3);
            tbFooter.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            tbFooter.DefaultCell.Border = 0;

            tbFooter.AddCell(new Paragraph());

            pCell = new PdfPCell(new Paragraph("Página " + writer.PageNumber));
            pCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pCell.Border = 0;
            tbFooter.AddCell(pCell);

            tbFooter.AddCell(new Paragraph());
            tbFooter.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetBottom(document.BottomMargin) - 5, writer.DirectContent);

            //Begin Image

            Image logo = Image.GetInstance(LogoPath);
            logo.SetAbsolutePosition(document.LeftMargin, writer.PageSize.GetTop(document.TopMargin + 5) + 10);
            logo.ScaleAbsolute(70f, 40f);

            document.Add(logo);

            //End Image

        }
    }
}