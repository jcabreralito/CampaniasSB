using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    [Table("Esquemas")]
    public class Esquema
    {
        [Key]
        public int EsquemaId { get; set; }

        [Display(Name = "Tipo Esquema")]
        public string TipoEsquema { get; set; }

        [Display(Name = "Esquema")]
        public string NombreEsquema { get; set; }

    }
}