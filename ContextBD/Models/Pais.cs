using System;
using System.Collections.Generic;

namespace ContextBD.Models
{
    public partial class Pais
    {
        public Pais()
        {
            Clinicas = new HashSet<Clinicas>();
            Users = new HashSet<Users>();
        }

        public int IdPais { get; set; }
        public string Iso { get; set; }
        public string Nombre { get; set; }

        public ICollection<Clinicas> Clinicas { get; set; }
        public ICollection<Users> Users { get; set; }
    }
}
