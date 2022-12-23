using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class LoginPatient
    {
        public string PatientPhoneNumber { get; set; }
        public string PatientPassword { get; set; }
        public string SecretKey { get; set; }
    }
}
