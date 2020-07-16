using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSharedMed.Models
{
    public class ExperticiaUsers
    {
        public string Exp { get; set; }
        public string Result { get; set; }
    }

    public class ExperticiaUsersData
    {

        public int IdUser { get; set; }
        public int IdExp { get; set; }
        public string Result { get; set; }

    }

    public class ExperticiaUsersPost
    {

        public List<ExperticiaUsersPostJson> data { get; set; }
        public List<UsersBancosData> dataBanco { get; set; }
    }
    public class ExperticiaUsersPostJson
    {

        public string Exp { get; set; }
        public string Result { get; set; }

    }
}
