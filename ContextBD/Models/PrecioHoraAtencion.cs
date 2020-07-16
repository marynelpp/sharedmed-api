using System;
using System.Collections.Generic;

namespace ContextBD.Models
{
    public partial class PrecioHoraAtencion
    {
        public PrecioHoraAtencion()
        {
            UserBanco = new HashSet<UserBanco>();
        }

        public int IdPrecio { get; set; }
        public decimal? Valor { get; set; }

        public ICollection<UserBanco> UserBanco { get; set; }
    }
}
