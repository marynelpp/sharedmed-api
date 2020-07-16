using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSharedMed.Models
{
    public class ServicioSearchResult
    {
        public string status { get; set; }
        public string Name { get; set; }
        public string Especialidad { get; set; }
        public string Idioma { get; set; }
        public string Pais { get; set; }
        public decimal? ValorServicio{ get; set; }
        public int? IdUserSolicitanteService { get; set; }
        public int? IdUserService { get; set; }
        public List<ServicioData> dataServicio { get; set; }
    }

    public class ServicioData
    {
        public int? IdUser { get; set; }
        public string Prioridad { get; set; }
        public string Idioma { get; set; }
        public int? IdEspMad { get; set; }
        public int? IdSubEsp { get; set; }
        public string Experticias { get; set; }
        public string Tiposervicio { get; set; }
        public string Tituloservicio { get; set; }
        public string Descripcioncaso { get; set; }
        public int? IdUserService { get; set; }
    }
}
