using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    [Table("CampañaArticulos")]
    public class CampañaArticulo
    {
        [Key]
        public long CampañaArticuloId { get; set; }

        [Display(Name = "Articulo Id")]
        public int ArticuloId { get; set; }

        [Display(Name = "Tienda Id")]
        public int TiendaId { get; set; }

        public int CampañaId { get; set; }

        public bool Habilitado { get; set; }

        public double Cantidad { get; set; }

    }
}