using System;
using System.Collections.Generic;

namespace ContextBD.Models
{
    public partial class SubEspOdo
    {
        public SubEspOdo()
        {
            ExperticiasEspecialidades = new HashSet<ExperticiasEspecialidades>();
            Users = new HashSet<Users>();
        }

        public int IdSubEspOdo { get; set; }
        public string Descrpcion { get; set; }

        public ICollection<ExperticiasEspecialidades> ExperticiasEspecialidades { get; set; }
        public ICollection<Users> Users { get; set; }
    }
}
