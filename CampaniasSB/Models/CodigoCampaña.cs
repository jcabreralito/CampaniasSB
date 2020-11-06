using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CampaniasSB.Models
{
    [Table("CodigosCampaña")]
    public class CodigoCampaña
    {
        [key]
        public int CodigoCampañaId { get; set; }

        public int ArticuloKFCId { get; set; }

        public int CampañaId { get; set; }

        public int Codigo { get; set; }

        public virtual Articulo ArticuloKFC { get; set; }

        public virtual Campaña Campaña { get; set; }
    }
}