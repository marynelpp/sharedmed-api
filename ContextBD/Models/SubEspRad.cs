using System;
using System.Collections.Generic;

namespace ContextBD.Models
{
    public partial class SubEspRad
    {
        public SubEspRad()
        {
            ExperticiasEspecialidades = new HashSet<ExperticiasEspecialidades>();
            Users = new HashSet<Users>();
        }

        public int IdSubEspRad { get; set; }
        public string Descripcion { get; set; }

        public ICollection<ExperticiasEspecialidades> ExperticiasEspecialidades { get; set; }
        public ICollection<Users> Users { get; set; }
    }
}
