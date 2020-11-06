using System.ComponentModel.DataAnnotations;

namespace CampaniasSB.Models
{
    public class CampañaArticulo
    {
        [Key]
        public long CampañaArticuloTMPId { get; set; }

        //public string Usuario { get; set; }

        //public int Compañia { get; set; }

        [Display(Name = "Articulo Id")]
        public int ArticuloKFCId { get; set; }

        [Display(Name = "Tienda Id")]
        public int TiendaId { get; set; }

        public int CampañaId { get; set; }

        public bool Habilitado { get; set; }

        public double Cantidad { get; set; }

        [Display(Name = "Código")]
        public int Codigo { get; set; }

        public virtual Articulo ArticuloKFC { get; set; }

    }
}