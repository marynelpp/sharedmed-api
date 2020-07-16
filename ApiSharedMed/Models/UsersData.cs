using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSharedMed.Models
{
    public class UsersData
    {
        public string Nombres { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public DateTime FechaNac { get; set; }
        public string RutPass { get; set; }
        public int IdNacionalidad { get; set; }
        public string Sexo { get; set; }
        public string Idioma { get; set; }
        public string NroCelular { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfPassword { get; set; }
        public int? IdRegion { get; set; }
        public int IdPais { get; set; }
        public int IdTipoUser { get; set; }
        public int IdEspecialidadMadre { get; set; }
        public int? IdSubEspQx { get; set; }
        public int? IdSubEspMed { get; set; }
        public int? IdSubEspPed { get; set; }
        public int? IdSubEspGobs { get; set; }
        public int? IdSubEspAnMedCr { get; set; }
        public int? IdSubEspRad { get; set; }
        public int? IdSubEspOdo { get; set; }
        public int? IdSubEspTec { get; set; }
        public int? IdClinica { get; set; }
        public string OtroLugarTrabajo { get; set; }
        public string OtraEsp { get; set; }
    }
}
