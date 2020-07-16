using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSharedMed.Models
{
    public class UsersBancos
    {
        public int IdUserBanco { get; set; }
        public int IdBanco { get; set; }
        public int IdPrecio { get; set; }
        public int IdUser { get; set; }
        public string NroCuenta { get; set; }

    }

    public class UsersBancosGet
    {
        public int IdUserBanco { get; set; }
        public int IdBanco { get; set; }
        public string Banco { get; set; }
        public int IdPrecio { get; set; }
        public decimal? Precio { get; set; }
        public string NroCuenta { get; set; }
        public int IdUser { get; set; }

    }
    public class UsersBancosData
    {
        public int IdBanco { get; set; }
        public int IdUser { get; set; }
        public string NroCuenta { get; set; }



    }
}
