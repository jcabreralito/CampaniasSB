using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    [Table("ReglasCaracteristicas")]
    public class ReglaCaracteristica
    {
        public int ReglaCaracteristicaId { get; set; }

        public int ReglaId { get; set; }

        public int ReglaCatalogoId { get; set; }

        public bool Seleccionado { get; set; }

        public bool IsTrue { get; set; }

        public bool IsFalse { get; set; }

    }
}