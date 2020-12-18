using CampaniasSB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.EnterpriseServices.Internal;
using System.Linq;

namespace CampaniasSB.Classes
{

    public class spMaterialesReglas
    {
        public int ArticuloId { get; set; }
        public string Descripcion { get; set; }
        public int ReglaId { get; set; }
        public int ReglaCatalogoId { get; set; }
        public bool Seleccionado { get; set; }
        public string Nombre { get; set; }
    }

    public class spMaterialesCaracteristicas
    {
        public int ArticuloId { get; set; }
        public int ReglaCatalogoId { get; set; }
        public string Nombre { get; set; }
        public string Habilitado { get; set; }
    }

    public class spArticulo
    {
        public int ArticuloId { get; set; }
        public string Descripcion { get; set; }
        public int CantidadDefault { get; set; }
        public string SencilloMultiple { get; set; }
        public string Observaciones { get; set; }
        public string Imagen { get; set; }
        public bool Eliminado { get; set; }
        public bool Activo { get; set; }
    }

    public class spReglas
    {
        public int ReglaId { get; set; }
        public string Material { get; set; }
        public int ArticuloId { get; set; }
        public int TipoId { get; set; }
        public string SencilloMultiple { get; set; }
    }

    public class spGetTiendaArticulos
    {
        public long TiendaArticuloId { get; set; }

        public int TiendaId { get; set; }

        public int ArticuloId { get; set; }

        public bool Seleccionado { get; set; }

        public int CantidadDefault { get; set; }

        public string Restaurante { get; set; }

        public string Material { get; set; }

    }

    public class MovementsHelper : IDisposable
    {
        private static readonly CampaniasContext db = new CampaniasContext();

        public void Dispose()
        {
            db.Dispose();
        }

        public static Response AgregarArticulosNuevaCampaña(int campañaid)
        {

            db.Database.ExecuteSqlCommand(
                "spAgregarTodosMaterialCampanias @CampañaId",
                new SqlParameter("@CampañaId", campañaid));

            return new Response { Succeeded = true, };
        }

        public static Response AgregarArticuloPorTiendas(int tiendaId, int campañaid)
        {
            db.Database.ExecuteSqlCommand(
            "spAgregarPorTiendaYCampania @TiendaId, @CampañaId",
            new SqlParameter("@TiendaId", tiendaId),
            new SqlParameter("@CampañaId", campañaid));

            return new Response { Succeeded = true, };

        }

        public static void AgregarMaterialesTiendaCampañaExiste(int articuloId, int tiendaId)
        {
            var campañas = db.Campañas.Where(x => x.Generada == "NO").ToList();

            if (articuloId == 0)
            {
                var tiendaArticulosExiste = db.Database.SqlQuery<TiendaArticulo>("spGetTiendaArticulos @TiendaId",
                    new SqlParameter("@TiendaId", tiendaId)).ToList();

                if (tiendaArticulosExiste.Count == 0)
                {
                    var articulos = db.Database.SqlQuery<Articulo>("spGetMaterialesAll").ToList();

                    foreach (var articulo in articulos)
                    {
                        AgregarNuevoMaterial(articulo.ArticuloId, tiendaId, articulo.Activo);
                    }

                    if (campañas.Count > 0)
                    {
                        foreach (var campaña in campañas)
                        {
                            AgregarArticuloPorTiendas(tiendaId, campaña.CampañaId);
                        }
                    }
                }
            }
            else
            {
                var articulosTiendasExiste = db.Database.SqlQuery<TiendaArticulo>("spGetArticuloTiendas @ArticuloId",
                    new SqlParameter("@ArticuloId", articuloId)).ToList();

                if (articulosTiendasExiste.Count == 0)
                {
                    var tiendas = db.Database.SqlQuery<Tienda>("spGetTiendasActivas").ToList();

                    foreach (var tienda in tiendas)
                    {
                        AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
                    }

                    if (campañas.Count > 0)
                    {
                        foreach (var campaña in campañas)
                        {
                            AgregarArticuloCampañas(articuloId, campaña.CampañaId);
                        }
                    }
                }
            }
        }

        private static void AgregarNuevoMaterial(int articuloId, int tiendaId, bool v)
        {
            var tiendaArticulos = db.TiendaArticulos.Where(cdt => cdt.ArticuloId == articuloId && cdt.TiendaId == tiendaId).FirstOrDefault();

            if (tiendaArticulos == null)
            {
                db.Database.ExecuteSqlCommand(
                "spAgregarTiendasMaterialC @ArticuloId, @TiendaId, @Seleccionado",
                                    new SqlParameter("@ArticuloId", articuloId),
                                    new SqlParameter("@TiendaId", tiendaId),
                                    new SqlParameter("@Seleccionado", v));
            }
        }

        public static Response AgregarArticuloCampañas(int articuloId, int campañaid)
        {

            db.Database.ExecuteSqlCommand(
            "spAgregarPorMaterialYCampania @ArticuloId, @CampañaId",
            new SqlParameter("@ArticuloId", articuloId),
            new SqlParameter("@CampañaId", campañaid));

            return new Response { Succeeded = true, };
        }

        public static Response GenerarCodigos(int? id)
        {

            try
            {
                var articulos = db.Database.SqlQuery<spArticulo>("spGetMaterialesCodigos").ToList();

                var familiasArt = articulos.GroupBy(f => new { f.SencilloMultiple }).ToList();

                foreach (var familia in familiasArt)
                {
                    var materialesFamilias = articulos.Where(x => x.SencilloMultiple == familia.Key.SencilloMultiple).ToList();

                    for (int f = 0; f < materialesFamilias.Count(); f++)
                    {
                        var articuloId = materialesFamilias[f].ArticuloId;

                        var codigosCampañas = db.CodigosCampaña.Where(cc => cc.ArticuloId == articuloId && cc.CampañaId == id).FirstOrDefault();

                        var idCampaña = db.Campañas.Where(c => c.CampañaId == id).FirstOrDefault().Nombre;

                        var consecutivo = string.Empty;

                        var codigo = 0;

                        if (codigosCampañas == null)
                        {
                            db.Database.ExecuteSqlCommand(
                            "spAgregarCodigos @ArticuloId, @CampañaId, @Codigo",
                            new SqlParameter("@ArticuloId", materialesFamilias[f].ArticuloId),
                            new SqlParameter("@CampañaId", (int)id),
                            new SqlParameter("@Codigo", codigo));

                        }
                    }
                }

                return new Response { Succeeded = true, };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Message = ex.Message,
                    Succeeded = false,
                };
            }
        }

        public static Response AgregarTiendaArticulos(int tiendaId)
        {
            var materialId = 0;

            var tiendas = db.Tiendas.Where(x => x.TiendaId == tiendaId).FirstOrDefault();

            var articulos = db.Articulos.ToList();

            AgregarMaterialesTiendaCampañaExiste(materialId, tiendaId);

            return new Response { Succeeded = true, };
        }

        public static Response ReglasTiendaArticulos(int tiendaId)
        {
            using (var transaccion = db.Database.BeginTransaction())
            {
                try
                {

                    var articulos = db.Articulos.ToList();

                    var tiendas = db.Tiendas.ToList();

                    foreach (var articulo in articulos)
                    {

                        var tiendaArticulos = db.TiendaArticulos.Where(cdt => cdt.ArticuloId == articulo.ArticuloId && cdt.TiendaId == tiendaId).FirstOrDefault();

                        if (tiendaArticulos == null)
                        {
                            var articuloDetalle = new TiendaArticulo
                            {
                                ArticuloId = articulo.ArticuloId,
                                Seleccionado = true,
                                TiendaId = tiendaId,
                            };

                            db.TiendaArticulos.Add(articuloDetalle);
                        }
                    }
                    db.SaveChanges();
                    transaccion.Commit();

                    return new Response { Succeeded = true, };
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    return new Response
                    {
                        Message = ex.Message,
                        Succeeded = false,
                    };
                }
            }
        }

        public static Response AgregarReglasCaracteristicas(string cat)
        {
            var caracteristicas = db.Database.SqlQuery<ReglaCatalogo>("spCaracterisiticas").ToList();

            if (cat == "EQUITY")
            {
                caracteristicas = db.Database.SqlQuery<ReglaCatalogo>("spCaracterisiticasEQ").ToList();
            }
            else if (cat == "FRANQUICIAS")
            {
                caracteristicas = db.Database.SqlQuery<ReglaCatalogo>("spCaracterisiticasFQ").ToList();
            }

            foreach (var caracteristica in caracteristicas)
            {
                var reglas = db.Database.SqlQuery<Regla>("spReglas").ToList();

                if (cat == "EQUITY")
                {
                    reglas = db.Database.SqlQuery<Regla>("spReglasEQ").ToList();
                }
                else if (cat == "FRANQUICIAS")
                {
                    reglas = db.Database.SqlQuery<Regla>("spReglasFQ").ToList();
                }

                if (reglas != null)
                {
                    foreach (var regla in reglas)
                    {
                        var existentes = db.Database.SqlQuery<ReglaCaracteristica>("spReglasCaracteristicasExistentes @ReglaId, @ReglaCatalogoId",
                            new SqlParameter("@ReglaId", regla.ReglaId),
                            new SqlParameter("@ReglaCatalogoId", caracteristica.ReglaCatalogoId)).FirstOrDefault();

                        if (existentes == null)
                        {
                            db.Database.ExecuteSqlCommand(
                            "spAgregarCaracteristicasReglas @ReglaId, @ReglaCatalogoId, @Seleccionado, @IsTrue, @IsFalse",
                            new SqlParameter("@ReglaId", regla.ReglaId),
                            new SqlParameter("@ReglaCatalogoId", caracteristica.ReglaCatalogoId),
                            new SqlParameter("@Seleccionado", false),
                            new SqlParameter("@IsTrue", false),
                            new SqlParameter("@IsFalse", false));
                        }
                    }
                }
            }
            return new Response { Succeeded = true, };
        }

        public static Response AgregarTiendasCaracteristicas(int reglaIdTienda, string cat, int fc, int fs, int il, int sb)
        {
            var tiendas = db.Tiendas.ToList();

            if (reglaIdTienda != 0)
            {
                foreach (var tienda in tiendas)
                {
                    var valor = "SI";

                    var existentes = db.TiendaCaracteristicas.Where(x => x.ReglaCatalogoId == reglaIdTienda && x.TiendaId == tienda.TiendaId).FirstOrDefault();

                    if (existentes == null)
                    {
                        db.Database.ExecuteSqlCommand(
                        "spAgregarTiendaCaracteristicas @TiendaId, @ReglaCatalogoId, @Valor",
                        new SqlParameter("@TiendaId", tienda.TiendaId),
                        new SqlParameter("@ReglaCatalogoId", reglaIdTienda),
                        new SqlParameter("@Valor", valor));
                    }
                }
            }
            else
            {
                foreach (var tienda in tiendas)
                {
                    var valor = "SI";

                    var existentes = db.TiendaCaracteristicas.Where(x => x.ReglaCatalogoId == reglaIdTienda && x.TiendaId == tienda.TiendaId).FirstOrDefault();

                    if (existentes == null)
                    {
                        db.Database.ExecuteSqlCommand(
                        "spAgregarTiendaCaracteristicas @TiendaId, @ReglaCatalogoId, @Valor",
                        new SqlParameter("@TiendaId", tienda.TiendaId),
                        new SqlParameter("@ReglaCatalogoId", reglaIdTienda),
                        new SqlParameter("@Valor", valor));
                    }
                }
            }
            return new Response { Succeeded = true, };
        }

        public static Response MovimientosBitacora(int usuarioId, string modulo, string movimiento)
        {
            db.Database.ExecuteSqlCommand(
            "spAgregarBitacora @Fecha, @UsuarioId, @Modulo, @Movimiento",
            new SqlParameter("@Fecha", DateTime.Now),
            new SqlParameter("@UsuarioId", usuarioId),
            new SqlParameter("@Modulo", modulo),
            new SqlParameter("@Movimiento", movimiento));

            return new Response { Succeeded = true, };
        }
    }
}