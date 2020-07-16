using System;
using System.Collections.Generic;

namespace ContextBD.Models
{
    public partial class ExperticiasEspecialidades
    {
        public int IdExp { get; set; }
        public int? IdEspMad { get; set; }
        public int? IdSubEspGobs { get; set; }
        public int? IdSubEspMed { get; set; }
        public int? IdSubEspPed { get; set; }
        public int? IdSubEspRad { get; set; }
        public int? IdSubEspAnMedCr { get; set; }
        public int? IdSubEspTec { get; set; }
        public int? IdSubEspOdo { get; set; }
        public int? IdSubEspQx { get; set; }
        public string Descripcion { get; set; }

        public EspecialidadMadre IdEspMadNavigation { get; set; }
        public SubEspAnMedCr IdSubEspAnMedCrNavigation { get; set; }
        public SubEspGobs IdSubEspGobsNavigation { get; set; }
        public SubEspMed IdSubEspMedNavigation { get; set; }
        public SubEspOdo IdSubEspOdoNavigation { get; set; }
        public SubEspPed IdSubEspPedNavigation { get; set; }
        public SubEspQx IdSubEspQxNavigation { get; set; }
        public SubEspRad IdSubEspRadNavigation { get; set; }
        public SubEspTec IdSubEspTecNavigation { get; set; }
    }
}
