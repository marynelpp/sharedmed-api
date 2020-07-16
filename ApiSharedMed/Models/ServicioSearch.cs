using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSharedMed.Models
{
    public class ServicioSearch
    {
        public int idUser { get; set; }
        public int idUserService { get; set; }
        public string Prioridad { get; set; }
        public string Idioma { get; set; }
        public int? IdEspMad { get; set; }
        public int? IdSubEsp { get; set; }
        public int[] Experticias { get; set; }
        public string Tiposervicio { get; set; }
        public string Tituloservicio { get; set; }
        public string Descripcioncaso { get; set; }
    }
    public class ServicioSearchData
    {
        public int idUser { get; set; }
        public int idUserService { get; set; }
        public string Prioridad { get; set; }
        public string Idioma { get; set; }
        public int? IdEspMad { get; set; }
        public int? IdSubEsp { get; set; }
        public string Experticias { get; set; }
        public string Tiposervicio { get; set; }
        public string Tituloservicio { get; set; }
        public string Descripcioncaso { get; set; }
    }

    public class ServicioHelperResult
    {
        public int idService { get; set; }
        public string UserSolicitante { get; set; }
        public string Prioridad { get; set; }
        public string Idioma { get; set; }
        public string EspMad { get; set; }
        public string SubEsp { get; set; }
        public string Tiposervicio { get; set; }
        public string Tituloservicio { get; set; }
        public string Descripcioncaso { get; set; }
        public string status { get; set; }
    }
}
