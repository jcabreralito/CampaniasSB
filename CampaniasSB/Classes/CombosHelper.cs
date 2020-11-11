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
            var regiones = db.Regiones.ToList();
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

        public static List<Ciudad> GetCiudades(int equiFran)
        {
            var ciudades = db.Ciudades.ToList();
            ciudades.Add(new Ciudad
            {
                CiudadId = 0,
                Nombre = "[Seleccionar...]",

            });

            return ciudades.OrderBy(r => r.Nombre).ToList();
        }

        public static List<Articulo> GetMateriales(int familiaId, bool sw)
        {
            var materiales = db.Articulos.ToList();
            return materiales.OrderBy(r => r.Descripcion).ToList();
        }

        public static List<Articulo> GetMateriales(string cat, bool sw)
        {
                var materiales = db.Database.SqlQuery<Articulo>("spGetReglasAsignadas").ToList();
  
                return materiales.OrderBy(r => r.Descripcion).ToList();

        }

        public static List<Articulo> GetMateriales(string cat)
        {
                var materiales = db.Articulos.Where(x => x.SencilloMultiple == cat && x.Eliminado == false).ToList();
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

        public static List<Esquema> GetNivelesPrecio()
        {
            var niveles = db.Esquemas.ToList();
            niveles.Add(new Esquema
            {
                EsquemaId = 0,
                TipoEsquema = "[Seleccionar...]",
            });
            return niveles.OrderBy(c => c.TipoEsquema).ToList();
        }

        public static List<Esquema> GetNivelesPrecio(bool sw)
        {
            var niveles = db.Esquemas.ToList();
            return niveles.OrderBy(c => c.TipoEsquema).ToList();
        }

    }
}