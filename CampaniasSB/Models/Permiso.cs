using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    [Table("Permisos")]
    public class Permiso
    {
        [Key]
        public int PermisoId { get; set; }

        public int CompañiaId { get; set; }

        public int UsuarioId { get; set; }

        public int ModuloId { get; set; }

    }
}