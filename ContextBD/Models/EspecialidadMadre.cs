using System;
using System.Collections.Generic;

namespace ContextBD.Models
{
    public partial class EspecialidadMadre
    {
        public EspecialidadMadre()
        {
            ExperticiasEspecialidades = new HashSet<ExperticiasEspecialidades>();
            Users = new HashSet<Users>();
        }

        public int IdEspMad { get; set; }
        public string Descripcion { get; set; }

        public ICollection<ExperticiasEspecialidades> ExperticiasEspecialidades { get; set; }
        public ICollection<Users> Users { get; set; }
    }
}
