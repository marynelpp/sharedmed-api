using System;
using System.Collections.Generic;

namespace ContextBD.Models
{
    public partial class ExperticiaUser
    {
        public int IdExpUser { get; set; }
        public int IdUser { get; set; }
        public int IdExp { get; set; }
        public string Result { get; set; }

        public Users IdUserNavigation { get; set; }
    }
}
