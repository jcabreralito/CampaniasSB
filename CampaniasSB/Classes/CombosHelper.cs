using CampaniasSB.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace CampaniasSB.Classes
{
    public class CombosHelper : IDisposable
    {
        private static readonly CampaniasContext db = new CampaniasContext();

        public static List<Compañia> GetCompañias(bool sw)
        {
            var compañias = db.Compañias.ToList();
            return compañias.OrderBy(c => c.Nombre).ToList();
        }

        public static List<Compañia> GetCompañias()
        {
            var compañias = db.Compañias.ToList();
            compañias.Add(new Compañia
            {
                CompañiaId = 0,
                Nombre = "[Seleccionar...]",
            });
            return compañias.OrderBy(c => c.Nombre).ToList();
        }

        public static List<Rol> GetRoles(bool sw)
        {
            var roles = db.Roles.ToList();
            return roles.OrderBy(r => r.Nombre).ToList();
        }

        public static List<Rol> GetRoles()
        {
            var roles = db.Roles.ToList();
            roles.Add(new Rol
            {
                RolId = 0,
                Nombre = "[Seleccionar...]",
            });
            return roles.OrderBy(r => r.Nombre).ToList();
        }

        public static List<Region> GetRegiones(int equiFran)
        {
            var tipo = string.Empty;

            if (equiFran == 1)
            {
                tipo = "EQUITY";
            }
            else if (equiFran == 2)
            {
                tipo = "FRANQUICIAS";
            }
            else if (equiFran == 3)
            {
                tipo = "STOCK";
            }

            var regiones = db.Regiones.Where(c => c.EquityFranquicia == tipo).ToList();
            regiones.Add(new Region
            {
                RegionId = 0,
                Nombre = "[Seleccionar...]",
            });
            return regiones.OrderBy(r => r.RegionId).ToList();
        }

        public static List<Region> GetRegiones(bool sw)
        {
            var regiones = db.Regiones.ToList();
            return regiones.OrderBy(r => r.Nombre).ToList();
        }

        public static List<Region> GetRegiones(int equiFran, bool sw)
        {
            var tipo = string.Empty;

            if (equiFran == 1)
            {
                tipo = "EQUITY";
            }
            else if (equiFran == 2)
            {
                tipo = "FRANQUICIAS";
            }
            else if (equiFran == 3)
            {
                tipo = "STOCK";
            }

            var regiones = db.Regiones.Where(c => c.EquityFranquicia == tipo).ToList();
            return regiones.OrderBy(r => r.Nombre).ToList();
        }

        public static List<Region> GetRegiones(string equiFran, bool sw)
        {
            var regiones = db.Regiones.Where(c => c.EquityFranquicia == equiFran).ToList();
            return regiones.OrderBy(r => r.Nombre).ToList();
        }

        public static List<TipoConfiguracion> GetTipoConfiguracion(bool v)
        {
            var configuraciones = db.TipoConfiguraciones.ToList();
            configuraciones.Add(new TipoConfiguracion
            {
                TipoConfiguracionId = 0,
                Nombre = "[Seleccionar...]",
            });
            return configuraciones.OrderBy(r => r.Nombre).ToList();
        }

        public static List<Ciudad> GetCiudades(int equiFran)
        {
            var tipo = string.Empty;

            if (equiFran == 1)
            {
                tipo = "EQUITY";
            }
            else if (equiFran == 2)
            {
                tipo = "FRANQUICIAS";
            }
            else if (equiFran == 3)
            {
                tipo = "STOCK";
            }

            var ciudades = db.Ciudades.Where(c => c.EquityFranquicia == tipo).ToList();
            ciudades.Add(new Ciudad
            {
                CiudadId = 0,
                Nombre = "[Seleccionar...]",

            });

            return ciudades.OrderBy(r => r.Nombre).ToList();
        }

        public static List<Articulo> GetMateriales(int familiaId, bool sw)
        {
            var materiales = db.Articulos.Where(x => x.FamiliaId == familiaId).ToList();
            return materiales.OrderBy(r => r.Descripcion).ToList();
        }

        public static List<Articulo> GetMateriales(string cat, bool sw)
        {
                var materiales = db.Database.SqlQuery<Articulo>("spGetReglasAsignadas").ToList();
  
                return materiales.OrderBy(r => r.Descripcion).ToList();

        }

        public static List<Articulo> GetMateriales(string cat)
        {
                var materiales = db.Articulos.Where(x => x.EquityFranquicia == cat && x.Eliminado == false).ToList();
                return materiales.OrderBy(r => r.Descripcion).ToList();
        }

        public static List<Articulo> GetMateriales(bool sw)
        {
            var materiales = db.Articulos.Where(x => x.Eliminado == false).ToList();
            return materiales.OrderBy(r => r.Descripcion).ToList();
        }

        public static List<Ciudad> GetCiudades(bool sw)
        {
            var ciudades = db.Ciudades.ToList();
            return ciudades.OrderBy(r => r.Nombre).ToList();
        }

        public static List<Ciudad> GetCiudades(string cat, bool sw)
        {
            var ciudades = db.Ciudades.Where(x => x.EquityFranquicia == cat).ToList();
            return ciudades.OrderBy(r => r.Nombre).ToList();
        }

        public static List<Ciudad> GetCiudades(int equiFran, bool sw)
        {
            var tipo = string.Empty;

            if (equiFran == 1)
            {
                tipo = "EQUITY";
            }
            else if (equiFran == 2)
            {
                tipo = "FRANQUICIAS";
            }
            else if (equiFran == 3)
            {
                tipo = "STOCK";
            }

            var ciudades = db.Ciudades.Where(c => c.EquityFranquicia == tipo).ToList();
            return ciudades.OrderBy(r => r.Nombre).ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public static List<ReglaCatalogo> GetTiposTienda(string cat, bool v)
        {
            var tipos = db.ReglasCatalogo.Where(x => x.Categoria == cat && x.Nombre.Equals("TIPO")).ToList();
            tipos.Add(new ReglaCatalogo
            {
                ReglaCatalogoId = 0,
                Valor = "[Seleccionar...]",
            });
            return tipos.OrderBy(c => c.Valor).ToList();
        }

        public static List<TipoTienda> GetTiposTienda(bool v)
        {
            var tipos = db.TipoTiendas.ToList();
            //tipos.Add(new TipoTienda
            //{
            //    TipoTiendaId = 0,
            //    Tipo = "[Seleccionar...]",
            //});
            return tipos.OrderBy(c => c.Tipo).ToList();
        }

        public static List<Familia> GetFamiliasC(bool v)
        {
            var familias = db.Familias.Where(x => x.Codigo == "995" && x.Codigo == "996" && x.Codigo == "997" && x.Codigo == "998" && x.Codigo == "999").ToList();
            return familias.OrderBy(c => c.Codigo).ToList();
        }

        public static List<Familia> GetFamiliasC()
        {
            var familias = db.Familias.Where(x => x.Codigo == "995" && x.Codigo == "996" && x.Codigo == "997" && x.Codigo == "998" && x.Codigo == "999").ToList();
            familias.Add(new Familia
            {
                FamiliaId = 0,
                Descripcion = "[Seleccionar...]",
            });
            return familias.OrderBy(c => c.Codigo).ToList();
        }

        public static List<Familia> GetFamilias(bool v)
        {
            var familias = db.Familias.Where(x => x.Codigo != "995" && x.Codigo != "996" && x.Codigo != "997" && x.Codigo != "998" && x.Codigo != "999").ToList();
            return familias.OrderBy(c => c.Codigo).ToList();
        }

        public static List<Familia> GetFamilias()
        {
            var familias = db.Familias.Where(x => x.Codigo != "995" && x.Codigo != "996" && x.Codigo != "997" && x.Codigo != "998" && x.Codigo != "999").ToList();
            familias.Add(new Familia
            {
                FamiliaId = 0,
                Descripcion = "[Seleccionar...]",
            });
            return familias.OrderBy(c => c.Codigo).ToList();
        }

        public static List<TipoCaja> GetTiposCaja(bool v)
        {
            var tiposCaja = db.TipoCajas.ToList();
            tiposCaja.Add(new TipoCaja
            {
                TipoCajaId = 0,
                Descripcion = "[Seleccionar...]",
            });
            return tiposCaja.OrderBy(c => c.Descripcion).ToList();
        }

        public static List<TipoCaja> GetTiposCaja()
        {
            var tiposCaja = db.TipoCajas.ToList();
            tiposCaja.Add(new TipoCaja
            {
                TipoCajaId = 0,
                Descripcion = "[Seleccionar...]",
            });
            return tiposCaja.OrderBy(c => c.Descripcion).ToList();
        }

        public static List<AcomodoCaja> GetAcomodoCajas(bool v)
        {
            var acomodos = db.AcomodoCajas.ToList();
            acomodos.Add(new AcomodoCaja
            {
                AcomodoCajaId = 0,
                Descripcion = "[Seleccionar...]",
            });
            return acomodos.OrderBy(c => c.Descripcion).ToList();
        }

        public static List<AcomodoCaja> GetAcomodoCajas()
        {
            var acomodos = db.AcomodoCajas.ToList();
            acomodos.Add(new AcomodoCaja
            {
                AcomodoCajaId = 0,
                Descripcion = "[Seleccionar...]",
            });
            return acomodos.OrderBy(c => c.Descripcion).ToList();
        }

        public static List<TipoTienda> GetTiposTienda()
        {
            var tipos = db.TipoTiendas.ToList();
            tipos.Add(new TipoTienda
            {
                TipoTiendaId = 0,
                Tipo = "[Seleccionar...]",
            });
            return tipos.OrderBy(c => c.Tipo).ToList();
        }

        public static List<NivelPrecio> GetNivelesPrecio()
        {
            var niveles = db.NivelPrecios.ToList();
            niveles.Add(new NivelPrecio
            {
                NivelPrecioId = 0,
                Descripcion = "[Seleccionar...]",
            });
            return niveles.OrderBy(c => c.Descripcion).ToList();
        }

        public static List<NivelPrecio> GetNivelesPrecio(bool sw)
        {
            var niveles = db.NivelPrecios.ToList();
            //niveles.Add(new NivelPrecio
            //{
            //    NivelPrecioId = 0,
            //    Descripcion = "[Seleccionar...]",
            //});
            return niveles.OrderBy(c => c.Descripcion).ToList();
        }

        public static List<Proveedor> GetProveedores(bool sw)
        {
            var proveedores = db.Proveedores.Where(x => x.ProveedorId != 5).ToList();
            return proveedores.OrderBy(c => c.Nombre).ToList();
        }

        public static List<TipoCampania> GetTipoCampañas(bool v)
        {
            var tipos = db.TipoCampanias.Where(x => x.Nombre != "STOCK").ToList();
            tipos.Add(new TipoCampania
            {
                TipoCampaniaId = 0,
                Nombre = "[Seleccionar...]",
            });
            return tipos.OrderBy(c => c.Nombre).ToList();
        }

        public static List<TipoCampania> GetTipoCampañasAll(bool v)
        {
            var tipos = db.TipoCampanias.ToList();
            //tipos.Add(new TipoCampania
            //{
            //    TipoCampaniaId = 0,
            //    Nombre = "[Seleccionar...]",
            //});
            return tipos.OrderBy(c => c.Nombre).ToList();
        }

        public static List<TipoCampania> GetTipoCampañasMat(bool v)
        {
            var tipos = db.TipoCampanias.Where(x => x.Nombre != "STOCK").ToList();
            return tipos.OrderBy(c => c.Nombre).ToList();
        }

        public static List<TiendaConfiguracion> GetConfiguracionesG(int id, string tipo, bool v)
        {
            var asignados = db.AsignarConfiguracionTiendas.Where(x => x.TiendaId == id && x.TiendaConfiguracionId == 3).ToList();

            if (asignados.Count == 0)
            {
                var configuraciones = db.TiendaConfiguraciones.Where(x => x.EquityFranquicia == tipo && x.Eliminado == false && x.TipoConfiguracion == "CARACTERISTICAS GENERALES").ToList();
                return configuraciones.OrderBy(r => r.Nombre).ToList();
            }
            else
            {
                var configuraciones = db.TiendaConfiguraciones.Where(x => x.EquityFranquicia == tipo && x.Eliminado == false && x.TipoConfiguracion == "CARACTERISTICAS GENERALES").ToList();

                foreach (var item in asignados)
                {
                    configuraciones = db.TiendaConfiguraciones.Where(x => x.EquityFranquicia == tipo && x.Eliminado == false && x.TipoConfiguracion == "CARACTERISTICAS GENERALES" && x.TiendaConfiguracionId != item.TiendaConfiguracionId).ToList();
                }
                return configuraciones.OrderBy(r => r.Nombre).ToList();
            }
        }
        public static List<TiendaConfiguracion> GetConfiguracionesP(string tipo, bool v)
        {
            var configuraciones = db.TiendaConfiguraciones.Where(x => x.EquityFranquicia == tipo && x.Eliminado == false && x.TipoConfiguracion == "CONFIGURACIÓN POR PRODUCTO").ToList();
            return configuraciones.OrderBy(r => r.Nombre).ToList();
        }
        public static List<TiendaConfiguracion> GetConfiguracionesME(string tipo, bool v)
        {
            var configuraciones = db.TiendaConfiguraciones.Where(x => x.EquityFranquicia == tipo && x.Eliminado == false && x.TipoConfiguracion == "CONFIGURACIÓN POR MATERIALES ESPECÍFICOS").ToList();
            return configuraciones.OrderBy(r => r.Nombre).ToList();
        }
        public static List<TiendaConfiguracion> GetConfiguracionesMedE(string tipo, bool v)
        {
            var configuraciones = db.TiendaConfiguraciones.Where(x => x.EquityFranquicia == tipo && x.Eliminado == false && x.TipoConfiguracion == "CONFIGURACIÓN POR MEDIDAS ESPECIALES").ToList();
            return configuraciones.OrderBy(r => r.Nombre).ToList();
        }
    }
}