using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    [Table("TiendaArticulos")]
    public class TiendaArticulo
    {
        [Key]
        public long TiendaArticuloId { get; set; }

        public int TiendaId { get; set; }

        public int ArticuloId { get; set; }

        public bool Seleccionado { get; set; }

    }
}