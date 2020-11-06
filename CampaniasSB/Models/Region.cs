using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    [Table("Regiones")]
    public class Region
    {
        [Key]
        public int RegionId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [MaxLength(250, ErrorMessage = "El Campo {0} debe tener máximo {1} carácteres de largo")]
        [Display(Name = "Región")]
        [Index("Región_Nombre_Index", IsUnique = true)]
        public string Nombre { get; set; }

        public string EquityFranquicia { get; set; }

    }
}