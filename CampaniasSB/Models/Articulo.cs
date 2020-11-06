using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace CampaniasSB.Models
{
    [Table("ArticulosKFC")]
    public class Articulo
    {
        [Key]
        public int ArticuloKFCId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [MaxLength(250, ErrorMessage = "El Campo {0} debe tener máximo {1} carácteres de largo")]
        [Display(Name = "Artículo")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccionar un {0}")]
        [Display(Name = "Proveedor", Prompt = "[Seleccionar...]")]
        public int ProveedorId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccionar una {0}")]
        [Display(Name = "Familia", Prompt = "[Seleccionar...]")]
        public int FamiliaId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [Display(Name = "Cantidad Default")]
        public int CantidadDefault { get; set; }

        public string EquityFranquicia { get; set; }

        public string Observaciones { get; set; }

        public bool Eliminado { get; set; }

        public bool Activo { get; set; }

        public bool Todo { get; set; }

        [Display(Name = "Liga Imagen")]
        public string LigaImagen { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string Imagen { get; set; }

        [NotMapped]
        [Display(Name = "Imagen")]
        public HttpPostedFileBase ImagenFile { get; set; }

        public virtual Proveedor Proveedor { get; set; }

        public virtual Familia Familia { get; set; }

    }
}