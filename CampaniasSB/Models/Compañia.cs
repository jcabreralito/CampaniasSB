using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace CampaniasSB.Models
{
    [Table("Compañias")]
    public class Compañia
    {
        [Key]
        public int CompañiaId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [MaxLength(250, ErrorMessage = "El Campo {0} debe tener máximo {1} carácteres de largo")]
        [Display(Name = "Compañia")]
        [Index("Compañia_Nombre_Index", IsUnique = true)]
        public string Nombre { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Logo")]
        public string Logo { get; set; }

        [NotMapped]
        [Display(Name = "Logo")]
        public HttpPostedFileBase LogoFile { get; set; }

    }
}