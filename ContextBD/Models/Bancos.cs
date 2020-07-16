using System;
using System.Collections.Generic;

namespace ContextBD.Models
{
    public partial class Bancos
    {
        public Bancos()
        {
            UserBanco = new HashSet<UserBanco>();
        }

        public int IdBanco { get; set; }
        public string Descripcion { get; set; }

        public ICollection<UserBanco> UserBanco { get; set; }
    }
}
