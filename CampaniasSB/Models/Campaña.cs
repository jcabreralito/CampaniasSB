using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    [Table("Campañas")]
    public class Campaña
    {
        [Key]
        public int CampañaId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [MaxLength(250, ErrorMessage = "El Campo {0} debe tener máximo {1} carácteres de largo")]
        [Display(Name = "ID")]
        [Index("Campaña_Nombre_Index", IsUnique = true)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [Display(Name = "Generada")]
        public string Generada { get; set; }

        //public virtual ICollection<CampañaArticulo> CampañaArticulos { get; set; }

        //public virtual ICollection<CodigoCampaña> CodigoCampañas { get; set; }

        //public List<CampañaArticuloTMP> CampañaDetalles { get; set; }

        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
        //public double TotalCantidad { get { return CampañaDetalles == null ? 0 : CampañaDetalles.Sum(d => d.Cantidad); } }



    }
}