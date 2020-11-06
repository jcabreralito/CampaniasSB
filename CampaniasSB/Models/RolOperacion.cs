using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CampaniasSB.Models
{
    [Table("RolOperaciones")]
    public class RolOperacion
    {
        [Key]
        public int RolOperacionId { get; set; }

        public int RolId { get; set; }

        public int OperacionId { get; set; }

        public virtual Rol Rol { get; set; }

        public virtual Operacion Operacion { get; set; }

    }
}