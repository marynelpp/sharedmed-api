using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSharedMed.Models
{
    public class UsersResult
    {
        public int IdUser { get; set; }
        public string Nombres { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string FechaNac { get; set; }
        public string RutPass { get; set; }
        public string Nacionalidad { get; set; }
        public string Sexo { get; set; }
        public string NroCelular { get; set; }
        public string Email { get; set; }
        public string Region { get; set; }
        public string Pais { get; set; }
        public string TipoUser { get; set; }
        public string EspecialidadMadre { get; set; }
        public string SubEsp { get; set; }
        public string LugarTrabajo { get; set; }
        public string OtroLugarTrabajo { get; set; }
    }
}
