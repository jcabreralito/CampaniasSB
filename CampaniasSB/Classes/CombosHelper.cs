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

        public class spCiudades
        {
            public int CiudadId { get; set; }
            public string Nombre { get; set; }
            public string Region { get; set; }
        }

        public static List<Compañia> GetCompañias(bool sw)
        {
            var compañias = db.Database.SqlQuery<Compañia>("spGetCompanias").ToList();
            return compañias.OrderBy(c => c.Nombre).ToList();
        }

        public static List<Compañia> GetCompañias()
        {
            var compañias = db.Database.SqlQuery<Compañia>("spGetCompanias").ToList();
            compañias.Add(new Compañia
            {
                CompañiaId = 0,
                Nombre = "[Seleccionar...]",
            });
            return compañias.OrderBy(c => c.Nombre).ToList();
        }

        public static List<Rol> GetRoles(bool sw)
        {
            var roles = db.Database.SqlQuery<Rol>("spGetRoles").ToList();
            return roles.OrderBy(r => r.Nombre).ToList();
        }

        public static List<Rol> GetRoles()
        {
            var roles = db.Database.SqlQuery<Rol>("spGetRoles").ToList();
            roles.Add(new Rol
            {
                RolId = 0,
                Nombre = "[Seleccionar...]",
            });
            return roles.OrderBy(r => r.Nombre).ToList();
        }

        public static List<Region> GetRegiones(int equiFran)
        {
            var regiones = db.Database.SqlQuery<Region>("spGetRegiones").ToList();
            regiones.Add(new Region
            {
                RegionId = 0,
                Nombre = "[Seleccionar...]",
            });
            return regiones.OrderBy(r => r.RegionId).ToList();
        }

        public static List<Region> GetRegiones(bool sw)
        {
            var regiones = db.Database.SqlQuery<Region>("spGetRegiones").ToList();
            return regiones.OrderBy(r => r.Nombre).ToList();
        }

        public static List<spCiudades> GetCiudades(int equiFran)
        {
            var ciudades = db.Database.SqlQuery<spCiudades>("spGetCiudades").ToList();
            ciudades.Add(new spCiudades
            {
                CiudadId = 0,
                Nombre = "[Seleccionar...]",

            });

            return ciudades.OrderBy(r => r.Nombre).ToList();
        }

        public static List<Articulo> GetMateriales(int familiaId, bool sw)
        {
            var materiales = db.Database.SqlQuery<Articulo>("spGetMaterialesAll").ToList();
            return materiales.OrderBy(r => r.Descripcion).ToList();
        }

        public static List<Articulo> GetMateriales(string cat, bool sw)
        {
                var materiales = db.Database.SqlQuery<Articulo>("spGetReglasAsignadas").ToList();
  
                return materiales.OrderBy(r => r.Descripcion).ToList();

        }

        public static List<Articulo> GetMateriales(string cat)
        {
                var materiales = db.Database.SqlQuery<Articulo>("spGetMaterialesAll").Where(x => x.SencilloMultiple == cat).ToList();
                return materiales.OrderBy(r => r.Descripcion).ToList();
        }

        public static List<Articulo> GetMateriales(bool sw)
        {
            var materiales = db.Database.SqlQuery<Articulo>("spGetMaterialesAll").ToList();
            return materiales.OrderBy(r => r.Descripcion).ToList();
        }

        public static List<spCiudades> GetCiudades(bool sw)
        {
            var ciudades = db.Database.SqlQuery<spCiudades>("spGetCiudades").ToList();
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

        public static List<Esquema> GetEsquemas()
        {
            var esquemas = db.Database.SqlQuery<Esquema>("spGetEsquemas").Where(x => x.TipoEsquema == "ESQUEMA GENERAL").ToList();
            esquemas.Add(new Esquema
            {
                EsquemaId = 0,
                NombreEsquema = "[Seleccionar...]",
            });
            return esquemas.OrderBy(c => c.NombreEsquema).ToList();
        }

        public static List<Esquema> GetEsquemas(bool sw)
        {
            var esquemas = db.Database.SqlQuery<Esquema>("spGetEsquemas").Where(x => x.TipoEsquema == "ESQUEMA GENERAL").ToList();
            return esquemas.OrderBy(c => c.NombreEsquema).ToList();
        }

        public static List<TipoArticulo> GetSencilloMultiple(bool v)
        {
            var tipo = db.Database.SqlQuery<TipoArticulo>("spGetTipoArticulo").ToList();
            tipo.Add(new TipoArticulo
            {
                TipoArticuloId = 0,
                Nombre = "[Seleccionar...]",
            });
            return tipo.OrderBy(c => c.Nombre).ToList();
        }

        public static List<Esquema> GetEsquemasCGG()
        {
            var esquemas = db.Database.SqlQuery<Esquema>("spGetEsquemas").Where(x => x.TipoEsquema == "ESQUEMA CENEFA GRAB AND GO").ToList();
            esquemas.Add(new Esquema
            {
                EsquemaId = 0,
                NombreEsquema = "[Seleccionar...]",
            });
            return esquemas.OrderBy(c => c.NombreEsquema).ToList();
        }
        public static List<Esquema> GetEsquemasCGG(bool v)
        {
            var esquemas = db.Database.SqlQuery<Esquema>("spGetEsquemas").Where(x => x.TipoEsquema == "ESQUEMA CENEFA GRAB AND GO").ToList();
            return esquemas.OrderBy(c => c.NombreEsquema).ToList();
        }
    }
}