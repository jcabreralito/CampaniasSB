using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    [Table("TiposCaja")]
    public class TipoCaja
    {
        [Key]
        public int TipoCajaId { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

    }
}