using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [MaxLength(250, ErrorMessage = "El Campo {0} debe tener máximo {1} carácteres de largo")]
        [Display(Name = "E-Mail")]
        [Index("Usuario_NombreUsuario_Index", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [Display(Name = "Nombre(s)")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccionar un {0}")]
        [Display(Name = "Rol", Prompt = "[Seleccionar un Rol...]")]
        public int RolId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccionar una {0}")]
        [Display(Name = "Compañia", Prompt = "[Seleccionar una Compañia...]")]
        public int CompañiaId { get; set; }

        [Display(Name = "Usuario")]
        public string NombreCompleto => string.Format("{0} {1}", Nombres, Apellidos);

        [JsonIgnore]
        public virtual Rol Rol { get; set; }

        [JsonIgnore]
        public virtual Compañia Compañia { get; set; }

    }
}