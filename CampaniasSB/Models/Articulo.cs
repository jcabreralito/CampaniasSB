using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace CampaniasSB.Models
{
    [Table("Articulos")]
    public class Articulo
    {
        [Key]
        public int ArticuloId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [MaxLength(250, ErrorMessage = "El Campo {0} debe tener máximo {1} carácteres de largo")]
        [Display(Name = "Artículo")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [Display(Name = "Cantidad Default")]
        public int CantidadDefault { get; set; }

        public string SencilloMultiple { get; set; }

        public string Observaciones { get; set; }

        public bool Eliminado { get; set; }

        public bool Activo { get; set; }

        [Display(Name = "Liga Imagen")]
        public string LigaImagen { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string Imagen { get; set; }

        [NotMapped]
        [Display(Name = "Imagen")]
        public HttpPostedFileBase ImagenFile { get; set; }

    }
}