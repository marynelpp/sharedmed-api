using System;
using System.Collections.Generic;

namespace ContextBD.Models
{
    public partial class TipoUser
    {
        public TipoUser()
        {
            Users = new HashSet<Users>();
        }

        public int IdTipoUser { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionInterna { get; set; }

        public ICollection<Users> Users { get; set; }
    }
}
