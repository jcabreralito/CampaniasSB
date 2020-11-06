using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    [Table("TiendaCaracteristicas")]
    public class TiendaCaracteristica
    {
        [key]
        public int TiendaCaracteristicaId { get; set; }

        public int TiendaId { get; set; }

        public int ReglaCatalogoId { get; set; }

        public string Valor { get; set; }

        public virtual Tienda Tienda { get; set; }

        public virtual ReglaCatalogo ReglaCatalogo { get; set; }

    }
}