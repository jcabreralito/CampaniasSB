using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CampaniasSB.Models
{
    [Table("Operaciones")]
    public class Operacion
    {
        [Key]
        public int OperacionId { get; set; }

        public string Nombre { get; set; }

        public int ModuloId { get; set; }

        public virtual Modulo Modulo { get; set; }

    }
}