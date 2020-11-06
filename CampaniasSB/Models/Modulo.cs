using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CampaniasSB.Models
{
    [Table("Modulos")]
    public class Modulo
    {
        [Key]
        public int ModuloId { get; set; }

        public string Nombre { get; set; }

    }
}