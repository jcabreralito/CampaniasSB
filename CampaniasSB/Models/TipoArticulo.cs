using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    [Table("TipoArticulos")]
    public class TipoArticulo
    {
        [key]
        public int TipoArticuloId { get; set; }

        public string Nombre { get; set; }
    }
}