using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSharedMed.Models
{
    public class TipoUsers
    {
        public int IdTipoUser { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionInt { get; set; }
    }

    public class TipoUsersData
    {
        public string Descripcion { get; set; }
        public string DescripcionInt { get; set; }
    }
}
