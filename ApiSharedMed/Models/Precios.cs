using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSharedMed.Models
{
    public class Precios
    {

        public int IdPrecio { get; set; }
        public decimal? Valor { get; set; }
    }
    public class PreciosData
    {

        public decimal? Valor { get; set; }
    }
}
