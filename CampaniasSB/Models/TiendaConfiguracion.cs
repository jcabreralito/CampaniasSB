using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    [Table("TiendaConfiguracion")]
    public class TiendaConfiguracion
    {
        [Key]
        public int TiendaConfiguracionId { get; set; }

        [Display(Name = "Caracteristica")]
        public string Nombre { get; set; }

        public string EquityFranquicia { get; set; }

        public string TipoConfiguracion { get; set; }

        public bool Eliminado { get; set; }
    }
}