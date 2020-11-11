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
        public string LigaImagen { get; set; }
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
            var tiendasTodas = db.Database.SqlQuery<Tienda>("spGetRestaurantes").Where(x => x.Activo == true).ToList();

            var articulos = db.Database.SqlQuery<Articulo>("spGetMaterialesActivos").ToList();

            if (articuloId != 0)
            {
                articulos = db.Articulos.Where(x => x.ArticuloId == articuloId).ToList();
            }

            if (tiendaId != 0)
            {
                tiendasTodas = db.Database.SqlQuery<Tienda>("spGetRestaurantes").Where(x => x.TiendaId == tiendaId).ToList();

            }

            var cumpleRegla = false;
            var cumpleReglaTipo = false;

            foreach (var articulo in articulos)
            {
                var reglas = db.Database.SqlQuery<spMaterialesCaracteristicas>("spGetMaterialCarateristicas @ArticuloKFCId",
                     new SqlParameter("@ArticuloKFCId", articulo.ArticuloId)).ToList();

                foreach (var tienda in tiendasTodas)
                {
                    var tipoRestaurante = string.Empty;

                    foreach (var regla in reglas)
                    {
                        var caracteristicasTienda = db.Database.SqlQuery<TiendaCaracteristica>("spGetCaracteristicasTienda @TiendaId, @ReglaCatalogoId",
                            new SqlParameter("@TiendaId", tienda.TiendaId),
                            new SqlParameter("@ReglaCatalogoId", regla.ReglaCatalogoId)).FirstOrDefault();

                        if (caracteristicasTienda == null)
                        {
                            cumpleRegla = false;
                            break;
                        }
                        else if (regla.Nombre == "FC" || regla.Nombre == "FS" || regla.Nombre == "IL" || regla.Nombre == "SB")
                        {
                            if (cumpleReglaTipo == true)
                            {
                                cumpleRegla = true;
                            }
                            else
                            {
                                if (regla.Nombre == tipoRestaurante)
                                {
                                    cumpleReglaTipo = true;
                                    cumpleRegla = true;
                                }
                                else
                                {
                                    cumpleReglaTipo = false;
                                    cumpleRegla = false;
                                }
                            }
                        }
                        else if (regla.Nombre.Contains("TIPO"))
                        {
                            tipoRestaurante = caracteristicasTienda.Valor;
                        }
                        else if (caracteristicasTienda.Valor != regla.Habilitado)
                        {
                            cumpleRegla = false;
                            break;
                        }
                        else
                        {
                            cumpleRegla = true;
                        }
                    }

                    if (cumpleRegla && cumpleReglaTipo)
                    {
                        AgregarNuevoMaterial(articulo.ArticuloId, tienda.TiendaId, true);
                        cumpleRegla = false;
                        cumpleReglaTipo = false;
                    }
                    else if (cumpleRegla == false && cumpleReglaTipo == false)
                    {
                        AgregarNuevoMaterial(articulo.ArticuloId, tienda.TiendaId, false);
                        cumpleRegla = false;
                        cumpleReglaTipo = false;
                    }
                    else if (cumpleRegla)
                    {
                        AgregarNuevoMaterial(articulo.ArticuloId, tienda.TiendaId, true);
                        cumpleRegla = false;
                        cumpleReglaTipo = false;
                    }
                    else if (cumpleRegla == false)
                    {
                        AgregarNuevoMaterial(articulo.ArticuloId, tienda.TiendaId, false);
                        cumpleRegla = false;
                        cumpleReglaTipo = false;
                    }

                }

            }

        }

        //private static void AsignarNumeroMotoboard(int articuloId, Tienda tienda)
        //{
        //    if (articuloId == 245) //245
        //    {
        //        if (tienda.TiendaId == 268 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 246) //246
        //    {
        //        if (tienda.TiendaId == 334 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 247) //247
        //    {
        //        if (tienda.TiendaId == 390 && tienda.TelefonoPersonalizado == true || tienda.TiendaId == 391 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 248) //248
        //    {
        //        if (tienda.TiendaId == 286 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 249) //249
        //    {
        //        if (tienda.TiendaId == 287 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 250) //250
        //    {
        //        if (tienda.TiendaId == 288 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 251) //251
        //    {
        //        if (tienda.TiendaId == 290 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 252) //252
        //    {
        //        if (tienda.TiendaId == 291 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 253) //253
        //    {
        //        if (tienda.TiendaId == 297 && tienda.TelefonoPersonalizado == true || tienda.TiendaId == 384 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 254) //254
        //    {
        //        if (tienda.TiendaId == 299 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 255) //255
        //    {
        //        if (tienda.TiendaId == 300 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 256) //256
        //    {
        //        if (tienda.TiendaId == 304 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 257) //257
        //    {
        //        if (tienda.TiendaId == 307 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 258) //258
        //    {
        //        if (tienda.TiendaId == 308 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 259) //259
        //    {
        //        if (tienda.TiendaId == 309 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 260) //260
        //    {
        //        if (tienda.TiendaId == 310 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 261) //261
        //    {
        //        if (tienda.TiendaId == 314 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 262) //262
        //    {
        //        if (tienda.TiendaId == 319 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 263) //263
        //    {
        //        if (tienda.TiendaId == 327 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 264) //264
        //    {
        //        if (tienda.TiendaId == 328 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 265) //265
        //    {
        //        if (tienda.TiendaId == 329 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 266) //266
        //    {
        //        if (tienda.TiendaId == 335 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 267) //267
        //    {
        //        if (tienda.TiendaId == 336 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 268) //268
        //    {
        //        if (tienda.TiendaId == 337 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 269) //269
        //    {
        //        if (tienda.TiendaId == 339 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 270) //270
        //    {
        //        if (tienda.TiendaId == 343 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 271) //271
        //    {
        //        if (tienda.TiendaId == 347 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 272) //272
        //    {
        //        if (tienda.TiendaId == 349 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 273) //273
        //    {
        //        if (tienda.TiendaId == 351 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 274) //274
        //    {
        //        if (tienda.TiendaId == 356 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 275) //275
        //    {
        //        if (tienda.TiendaId == 367 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 276) //276
        //    {
        //        if (tienda.TiendaId == 368 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 277) //277
        //    {
        //        if (tienda.TiendaId == 373 && tienda.TelefonoPersonalizado == true || tienda.TiendaId == 379 && tienda.TelefonoPersonalizado == true || tienda.TiendaId == 382 && tienda.TelefonoPersonalizado == true)
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //        }
        //        else
        //        {
        //            AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //        }
        //    }

        //    if (articuloId == 278) //278
        //    {
        //        {
        //            if (tienda.TiendaId == 374 && tienda.TelefonoPersonalizado == true)
        //            {
        //                AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //            }
        //            else
        //            {
        //                AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //            }
        //        }

        //        if (articuloId == 280) //280
        //        {
        //            if (tienda.TiendaId == 394 && tienda.TelefonoPersonalizado == true)
        //            {
        //                AgregarNuevoMaterial(articuloId, tienda.TiendaId, true);
        //            }
        //            else
        //            {
        //                AgregarNuevoMaterial(articuloId, tienda.TiendaId, false);
        //            }
        //        }
        //    }
        //}

        private static void SiExisteMedidaEspecialAE(Tienda tienda, bool selecc, int articuloKFCId)
        {
            var tiendaArticulos = db.TiendaArticulos.Where(cdt => cdt.ArticuloId == articuloKFCId && cdt.TiendaId == tienda.TiendaId).FirstOrDefault();

            if (tiendaArticulos == null)
            {
                //if (tienda.MEDIDA_ESPECIAL_AE_CANDILES_49_5x73_5cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_CELAYA_50x68_5cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_MIRASIERRA_46x68cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_TECAMAC_48x67_5cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_VALLE_SOLEADO_51x71cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_VILLA_GARCIA_45x65cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_XOLA_49_9x66_9cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_ZUAZUA_51x71cm == true)
                //{
                //    selecc = false;
                //}
                //else
                //{
                //    selecc = true;
                //}

                AgregarNuevoMaterial(articuloKFCId, tienda.TiendaId, selecc);
            }
            else
            {
                //if (tienda.MEDIDA_ESPECIAL_AE_CANDILES_49_5x73_5cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_CELAYA_50x68_5cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_MIRASIERRA_46x68cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_TECAMAC_48x67_5cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_VALLE_SOLEADO_51x71cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_VILLA_GARCIA_45x65cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_XOLA_49_9x66_9cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_ZUAZUA_51x71cm == true)
                //{
                //    selecc = false;
                //}
                //else
                //{
                //    selecc = true;
                //}

                tiendaArticulos.Seleccionado = selecc;
                db.Entry(tiendaArticulos).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        private static void SiExisteMedidaEspecialPRE(Tienda tienda, bool selecc, int articuloKFCId)
        {
            var tiendaArticulos = db.TiendaArticulos.Where(cdt => cdt.ArticuloId == articuloKFCId && cdt.TiendaId == tienda.TiendaId).FirstOrDefault();

            if (tiendaArticulos == null)
            {
                //if (tienda.MEDIDA_ESPECIAL_AE_CANDILES_49_5x73_5cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_CELAYA_50x68_5cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_MIRASIERRA_46x68cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_TECAMAC_48x67_5cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_VALLE_SOLEADO_51x71cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_VILLA_GARCIA_45x65cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_XOLA_49_9x66_9cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_ZUAZUA_51x71cm == true ||
                //    tienda.MEDIDA_ESPECIAL_PRE_MENU_AE_SAN_ANTONIO_49x67_5cm == true)
                //{
                //    selecc = false;
                //}
                //else
                //{
                //    selecc = true;
                //}

                AgregarNuevoMaterial(articuloKFCId, tienda.TiendaId, selecc);
            }
            else
            {
                //if (tienda.MEDIDA_ESPECIAL_AE_CANDILES_49_5x73_5cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_CELAYA_50x68_5cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_MIRASIERRA_46x68cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_TECAMAC_48x67_5cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_VALLE_SOLEADO_51x71cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_VILLA_GARCIA_45x65cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_XOLA_49_9x66_9cm == true ||
                //    tienda.MEDIDA_ESPECIAL_AE_ZUAZUA_51x71cm == true ||
                //    tienda.MEDIDA_ESPECIAL_PRE_MENU_AE_SAN_ANTONIO_49x67_5cm == true)
                //{
                //    selecc = false;
                //}
                //else
                //{
                //    selecc = true;
                //}

                tiendaArticulos.Seleccionado = selecc;
                db.Entry(tiendaArticulos).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        private static void SiExisteMedidaEspecialWC(Tienda tienda, bool selecc, int articuloKFCId)
        {
            var tiendaArticulos = db.TiendaArticulos.Where(cdt => cdt.ArticuloId == articuloKFCId && cdt.TiendaId == tienda.TiendaId).FirstOrDefault();

            if (tiendaArticulos == null)
            {
                //if (tienda.EquityFranquicia == "EQUITY" || tienda.EquityFranquicia == "STOCK")
                //{
                //    if (tienda.WCMedidaEspecial60_8x85cm == true ||
                //        tienda.WCNACIONAL67X100cm == true ||
                //        tienda.WC_MEDIDA_ESPECIAL_CORREO_MAYOR_60x90cm == true ||
                //        tienda.WC_MEDIDA_ESPECIAL_MALL_ORIENTE_100x120cm == true ||
                //        tienda.WC_MEDIDA_ESPECIAL_ZARAGOZA_90x100cm == true ||
                //        tienda.WC_MEDIDA_ESPECIAL_ZUAZUA_87x120cm == true)
                //    {
                //        selecc = false;
                //    }
                //    else
                //    {
                //        selecc = true;
                //    }
                //}
                //else
                //{
                //    selecc = true;
                //}

                AgregarNuevoMaterial(articuloKFCId, tienda.TiendaId, selecc);
            }
            else
            {
                //if (tienda.EquityFranquicia == "EQUITY" || tienda.EquityFranquicia == "STOCK")
                //{
                //    if (tienda.WCMedidaEspecial60_8x85cm == true ||
                //        tienda.WCNACIONAL67X100cm == true ||
                //        tienda.WC_MEDIDA_ESPECIAL_CORREO_MAYOR_60x90cm == true ||
                //        tienda.WC_MEDIDA_ESPECIAL_MALL_ORIENTE_100x120cm == true ||
                //        tienda.WC_MEDIDA_ESPECIAL_ZARAGOZA_90x100cm == true ||
                //        tienda.WC_MEDIDA_ESPECIAL_ZUAZUA_87x120cm == true)
                //    {
                //        selecc = false;
                //    }
                //    else
                //    {
                //        selecc = true;
                //    }
                //}
                //else
                //{
                //    selecc = true;
                //}

                AgregarNuevoMaterial(articuloKFCId, tienda.TiendaId, selecc);

                tiendaArticulos.Seleccionado = selecc;
                db.Entry(tiendaArticulos).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        //private static void SiExisteMedidaEspecialZuaZua(Tienda tienda, bool selecc, int articuloKFCId)
        //{
        //    var tiendaArticulos = db.TiendaArticulos.Where(cdt => cdt.ArticuloId == articuloKFCId && cdt.TiendaId == tienda.TiendaId).FirstOrDefault();

        //    if (tiendaArticulos == null)
        //    {
        //        if (tienda.MEDIDA_ESPECIAL_AE_ZUAZUA_51x71cm == true)
        //        {
        //            selecc = false;
        //        }
        //        else
        //        {
        //            selecc = true;
        //        }

        //        AgregarNuevoMaterial(articuloKFCId, tienda.TiendaId, selecc);
        //    }
        //    else
        //    {
        //        if (tienda.MEDIDA_ESPECIAL_AE_ZUAZUA_51x71cm == true)
        //        {
        //            selecc = false;
        //        }
        //        else
        //        {
        //            selecc = true;
        //        }

        //        tiendaArticulos.Seleccionado = selecc;
        //        db.Entry(tiendaArticulos).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //}

        private static void NoSeleccionarMaterialPadre(int articuloKFCId, int tiendaId, bool v)
        {
            var tiendaArticulos = db.TiendaArticulos.Where(cdt => cdt.ArticuloId == articuloKFCId && cdt.TiendaId == tiendaId).FirstOrDefault();

            if (tiendaArticulos == null)
            {
                db.Database.ExecuteSqlCommand(
                "spAgregarTiendasMaterialC @ArticuloKFCId, @TiendaId, @Seleccionado",
                                new SqlParameter("@ArticuloKFCId", articuloKFCId),
                                new SqlParameter("@TiendaId", tiendaId),
                                new SqlParameter("@Seleccionado", v));
            }
            else
            {
                bool selec = v;

                tiendaArticulos.Seleccionado = selec;
                db.Entry(tiendaArticulos).State = EntityState.Modified;
                db.SaveChanges();

            }
        }

        private static void AgregarNuevoMaterial(int articuloKFCId, int tiendaId, bool v)
        {
            var tiendaArticulos = db.TiendaArticulos.Where(cdt => cdt.ArticuloId == articuloKFCId && cdt.TiendaId == tiendaId).FirstOrDefault();

            if (tiendaArticulos == null)
            {
                db.Database.ExecuteSqlCommand(
                "spAgregarTiendasMaterialC @ArticuloKFCId, @TiendaId, @Seleccionado",
                                    new SqlParameter("@ArticuloKFCId", articuloKFCId),
                                    new SqlParameter("@TiendaId", tiendaId),
                                    new SqlParameter("@Seleccionado", v));
            }
        }

        private static void AsignarMaterialF(int primero, int tiendaId)
        {
            var tiendaArticulos = db.TiendaArticulos.Where(cdt => cdt.ArticuloId == primero && cdt.TiendaId == tiendaId).FirstOrDefault();

            if (tiendaArticulos == null)
            {
                db.Database.ExecuteSqlCommand(
                "spAgregarTiendasMaterialC @ArticuloKFCId, @TiendaId, @Seleccionado",
                new SqlParameter("@ArticuloKFCId", primero),
                new SqlParameter("@TiendaId", tiendaId),
                new SqlParameter("@Seleccionado", true));
            }
            else
            {
                var selec = true;

                tiendaArticulos.Seleccionado = selec;
                db.Entry(tiendaArticulos).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static Response AgregarArticuloCampañas(Articulo articuloKFC, int campañaid)
        {

            db.Database.ExecuteSqlCommand(
            "spAgregarPorMaterialYCampania @ArticuloKFCId, @CampañaId",
            new SqlParameter("@ArticuloKFCId", articuloKFC.ArticuloId),
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

                        //var codigosTodosCampañas = db.CodigosCampaña.Where(cc => cc.CampañaId == id && cc.ArticuloKFC.Familia.Codigo == familia.Key.CodigoFamilia).ToList();
                        var codigo = 0;

                        //if (codigosTodosCampañas.Count > 0)
                        //{
                        //    var ultimoCodigo = db.CodigosCampaña.Where(x => x.ArticuloKFC.Familia.Codigo == familia.Key.CodigoFamilia && x.CampañaId == id).OrderByDescending(x => x.Codigo).FirstOrDefault();

                        //    codigo = ultimoCodigo.Codigo + 1;
                        //}
                        //else
                        //{
                        //    consecutivo = "000";
                        //    codigo = Convert.ToInt32(idCampaña + materialesFamilias[f].CodigoFamilia + consecutivo);
                        //}

                        if (codigosCampañas == null)
                        {
                            db.Database.ExecuteSqlCommand(
                            "spAgregarCodigos @ArticuloKFCId, @CampañaId, @Codigo",
                            new SqlParameter("@ArticuloKFCId", materialesFamilias[f].ArticuloId),
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