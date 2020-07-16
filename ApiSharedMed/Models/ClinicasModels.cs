using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSharedMed.Models
{
    public class ClinicasModel
    {
        public int IdClinica { get; set; }
        public int? IdPais { get; set; }
        public string Descripcion { get; set; }
    }
    public class ClinicasModelData
    {
        public int? IdPais { get; set; }
        public string Descripcion { get; set; }
    }
}
