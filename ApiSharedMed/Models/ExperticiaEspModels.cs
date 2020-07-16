using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSharedMed.Models
{
    public class ExperticiaEspModels
    {
        public int IdExp { get; set; }
        public int? IdEspMad { get; set; }
        public int? IdSubEspGobs { get; set; }
        public int? IdSubEspMed { get; set; }
        public int? IdSubEspPed { get; set; }
        public int? IdSubEspQx { get; set; }
        public int? IdSubEspAnMedCr { get; set; }
        public int? IdSubEspRad { get; set; }
        public int? IdSubEspOdo { get; set; }
        public int? IdSubEspTec { get; set; }
        public string Descripcion { get; set; }
    }

    public class ExperticiaEspModelsData
    {
        public int? IdEspMad { get; set; }
        public int? IdSubEspGobs { get; set; }
        public int? IdSubEspMed { get; set; }
        public int? IdSubEspPed { get; set; }
        public int? IdSubEspQx { get; set; }
        public int? IdSubEspAnMedCr { get; set; }
        public int? IdSubEspRad { get; set; }
        public int? IdSubEspOdo { get; set; }
        public int? IdSubEspTec { get; set; }
        public string Descripcion { get; set; }


    }
}
