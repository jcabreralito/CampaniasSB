using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    [Table("Errores")]
    public class Error
    {
        [Key]
        public int ErrorId { get; set; }

        public string NombreError { get; set; }

        public string Descripcion { get; set; }

        public string IconoFA { get; set; }

    }
}