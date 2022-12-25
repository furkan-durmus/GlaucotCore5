using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOS
{
    public class PatientDataTablesParam
    {
        public string TextSearch { get; set; }
        public int OrderCol { get; set; }
        public bool OrderDesc { get; set; }
        public int Start { get; set; }
        public int Size { get; set; }
    }
}
