using System;
using System.Collections.Generic;

namespace ContextBD.Models
{
    public partial class SubEspPed
    {
        public SubEspPed()
        {
            ExperticiasEspecialidades = new HashSet<ExperticiasEspecialidades>();
            Users = new HashSet<Users>();
        }

        public int IdSubEspPed { get; set; }
        public string Descripcion { get; set; }

        public ICollection<ExperticiasEspecialidades> ExperticiasEspecialidades { get; set; }
        public ICollection<Users> Users { get; set; }
    }
}
