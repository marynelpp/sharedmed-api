using System;
using System.Collections.Generic;

namespace ContextBD.Models
{
    public partial class Nacionalidad
    {
        public Nacionalidad()
        {
            Users = new HashSet<Users>();
        }

        public int IdNacionalidad { get; set; }
        public string PaisNac { get; set; }
        public string GentilicioNac { get; set; }
        public string IsoNac { get; set; }

        public ICollection<Users> Users { get; set; }
    }
}
