using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    [Table("Ciudades")]
    public class Ciudad
    {
        [Key]
        public int CiudadId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [Display(Name = "Ciudad")]
        public string Nombre { get; set; }

    }
}