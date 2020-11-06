using System.ComponentModel.DataAnnotations;

namespace CampaniasSB.Models
{
    public class TipoCampania
    {
        [Key]
        public int TipoCampaniaId { get; set; }

        public string Nombre { get; set; }

    }
}