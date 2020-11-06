using System.ComponentModel.DataAnnotations.Schema;

namespace CampaniasSB.Models
{
    [Table("ReglasCatalogoValor")]
    public class ReglaCatalogoValor
    {
        [key]
        public int ReglaCatalogoValorId { get; set; }

        public string Descripcion { get; set; }

        public string Valor { get; set; }

    }
}