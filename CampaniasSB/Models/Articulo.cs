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

        [MaxLength(250, ErrorMessage = "El Campo {0} debe tener máximo {1} carácteres de largo")]
        [Display(Name = "Artículo")]
        public string Descripcion { get; set; }

        [Display(Name = "Cantidad Default")]
        public int CantidadDefault { get; set; }

        public string SencilloMultiple { get; set; }

        public string Observaciones { get; set; }

        public bool Eliminado { get; set; }

        public bool Activo { get; set; }

        public bool Precio { get; set; }

        [Display(Name = "Bilingüe")]
        public bool Bilingue { get; set; }

        public string Medida { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string Imagen { get; set; }

        [NotMapped]
        [Display(Name = "Imagen")]
        public HttpPostedFileBase ImagenFile { get; set; }

    }
}