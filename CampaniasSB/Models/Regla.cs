using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    [Table("Reglas")]
    public class Regla
    {
        [Key]
        public int ReglaId { get; set; }

        [Display(Name = "Material", Prompt = "[Seleccionar...]")]
        public int ArticuloId { get; set; }

        public string NombreRegla { get; set; }

    }
}