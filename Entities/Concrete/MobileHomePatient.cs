using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class GeneralMobilePatientRequest
    {
        public Guid PatientId { get; set; }
        public string SecretKey { get; set; }
    }
}
