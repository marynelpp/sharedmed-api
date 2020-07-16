using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSharedMed.Models
{
    public class LoginResult
    {
        public string status { get; set; }
        public int idUser { get; set; }
        public string nameUser { get; set; }
        public string nameUserDr { get; set; }
        public string emailUser { get; set; }
        public int idEspMad { get; set; }
        public int? idSubEsp { get; set; }
        public int idPais { get; set; }
        public int? disponible { get; set; }
    }
}
