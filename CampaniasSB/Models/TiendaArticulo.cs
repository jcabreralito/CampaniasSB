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

        public int ArticuloKFCId { get; set; }

        public bool Seleccionado { get; set; }

        //public string EquityFranquicia { get; set; }

        public virtual Tienda Tienda { get; set; }

        public virtual Articulo ArticuloKFC { get; set; }

        public virtual CampañaArticulo CampañaArticuloTMP { get; set; }

    }
}