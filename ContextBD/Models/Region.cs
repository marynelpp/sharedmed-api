using System;
using System.Collections.Generic;

namespace ContextBD.Models
{
    public partial class Region
    {
        public Region()
        {
            Users = new HashSet<Users>();
        }

        public int IdRegion { get; set; }
        public string Descripcion { get; set; }

        public ICollection<Users> Users { get; set; }
    }
}
