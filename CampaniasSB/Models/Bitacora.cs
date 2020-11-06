using System;

namespace CampaniasSB.Models
{
    public class Bitacora
    {
        [key]
        public int BitacoraId { get; set; }

        public DateTime Fecha { get; set; }

        public int UsuarioId { get; set; }

        public string Modulo { get; set; }

        public string Movimiento { get; set; }
    }
}