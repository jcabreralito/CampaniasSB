using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    [Table("Tiendas")]
    public class Tienda
    {
        [Key]
        public int TiendaId { get; set; }

        public int EsquemaId { get; set; }

        public int EsquemaCGGId { get; set; }

        [Display(Name = "No. Tienda")]
        public string NoTienda { get; set; }

        [Display(Name = "Nombre de Tienda ")]
        public string NombreTienda { get; set; }

        [Display(Name = "REGIÓN", Prompt = "[Seleccionar...]")]
        public int RegionId { get; set; }

        [Display(Name = "CIUDAD", Prompt = "[Seleccionar...]")]
        public int CiudadId { get; set; }

        //[Display(Name = "DIRECCIÓN")]
        //public string Direccion { get; set; }

        public bool BIS { get; set; }

        public bool Idioma { get; set; }

        public string Observaciones { get; set; }

        public bool Activo { get; set; }

        public bool Eliminado { get; set; }

        public string Base { get; set; }

        public string Altura { get; set; }

        public string Especial { get; set; }

    }
}