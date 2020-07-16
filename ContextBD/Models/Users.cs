using System;
using System.Collections.Generic;

namespace ContextBD.Models
{
    public partial class Users
    {
        public Users()
        {
            ExperticiaUser = new HashSet<ExperticiaUser>();
            Servicio = new HashSet<Servicio>();
            UserBanco = new HashSet<UserBanco>();
        }

        public int IdUser { get; set; }
        public string Nombres { get; set; }
        public string PApellido { get; set; }
        public string SApellido { get; set; }
        public DateTime FechaNac { get; set; }
        public string RutPass { get; set; }
        public int IdNacionalidad { get; set; }
        public string Idioma { get; set; }
        public string Sexo { get; set; }
        public string NroCelular { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SaltCode { get; set; }
        public int? IdRegion { get; set; }
        public int IdPais { get; set; }
        public int IdEspMad { get; set; }
        public int? IdSubEspQx { get; set; }
        public int? IdSubEspMed { get; set; }
        public int? IdSubEspPed { get; set; }
        public int? IdSubEspAnMedCr { get; set; }
        public int? IdSubEspRad { get; set; }
        public int? IdSubEspTec { get; set; }
        public int? IdSubEspOdo { get; set; }
        public int? IdSubEspGobs { get; set; }
        public int? IdClinica { get; set; }
        public int IdTipoUser { get; set; }
        public string OtroLugTrab { get; set; }
        public string OtraEsp { get; set; }
        public int? Disponible { get; set; }
        public int? Helper { get; set; }

        public Clinicas IdClinicaNavigation { get; set; }
        public EspecialidadMadre IdEspMadNavigation { get; set; }
        public Nacionalidad IdNacionalidadNavigation { get; set; }
        public Pais IdPaisNavigation { get; set; }
        public Region IdRegionNavigation { get; set; }
        public SubEspAnMedCr IdSubEspAnMedCrNavigation { get; set; }
        public SubEspGobs IdSubEspGobsNavigation { get; set; }
        public SubEspMed IdSubEspMedNavigation { get; set; }
        public SubEspOdo IdSubEspOdoNavigation { get; set; }
        public SubEspPed IdSubEspPedNavigation { get; set; }
        public SubEspQx IdSubEspQxNavigation { get; set; }
        public SubEspRad IdSubEspRadNavigation { get; set; }
        public SubEspTec IdSubEspTecNavigation { get; set; }
        public TipoUser IdTipoUserNavigation { get; set; }
        public ICollection<ExperticiaUser> ExperticiaUser { get; set; }
        public ICollection<Servicio> Servicio { get; set; }
        public ICollection<UserBanco> UserBanco { get; set; }
    }
}
