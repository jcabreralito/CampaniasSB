using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    [Table("Familias")]
    public class Familia
    {
        public int FamiliaId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [MaxLength(250, ErrorMessage = "El Campo {0} debe tener máximo {1} carácteres de largo")]
        [Display(Name = "Descripción")]
        [Index("Familia_Descripcion_Index", IsUnique = true)]
        public string Descripcion { get; set; }

        [MaxLength(3, ErrorMessage = "El Campo {0} debe tener máximo {1} carácteres de largo")]
        [Display(Name = "Código")]
        public string Codigo { get; set; }

    }
}