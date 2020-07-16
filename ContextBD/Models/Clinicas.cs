using System;
using System.Collections.Generic;

namespace ContextBD.Models
{
    public partial class Clinicas
    {
        public Clinicas()
        {
            Users = new HashSet<Users>();
        }

        public int IdClinica { get; set; }
        public int? IdPais { get; set; }
        public string Descripcion { get; set; }

        public Pais IdPaisNavigation { get; set; }
        public ICollection<Users> Users { get; set; }
    }
}
