using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    [Table("NivelesPrecio")]
    public class NivelPrecio
    {
        [Key]
        public int NivelPrecioId { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

    }
}