using System;
using System.Collections.Generic;

namespace ContextBD.Models
{
    public partial class UserBanco
    {
        public int IdUserBanco { get; set; }
        public int IdBanco { get; set; }
        public int IdPrecio { get; set; }
        public int IdUser { get; set; }
        public string NroCuenta { get; set; }

        public Bancos IdBancoNavigation { get; set; }
        public PrecioHoraAtencion IdPrecioNavigation { get; set; }
        public Users IdUserNavigation { get; set; }
    }
}
