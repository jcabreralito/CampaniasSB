using System.ComponentModel.DataAnnotations;

namespace CampaniasSB.Models
{
    public class TipoConfiguracion
    {
        [Key]
        public int TipoConfiguracionId { get; set; }

        public string Nombre { get; set; }
    }
}