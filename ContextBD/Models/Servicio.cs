using System;
using System.Collections.Generic;

namespace ContextBD.Models
{
    public partial class Servicio
    {
        public int IdServicio { get; set; }
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
        public string Status { get; set; }

        public Users IdUserNavigation { get; set; }
    }
}
