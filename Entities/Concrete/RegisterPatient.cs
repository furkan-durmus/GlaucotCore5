using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class RegisterPatient
    {
        public string? PatientName { get; set; }
        public string? PatientLastName { get; set; }
        public string PatientPhoneNumber { get; set; }
        public string? PatientPassword { get; set; }
        public string? OTPCode { get; set; }
        public string SecretKey { get; set; }
    }
}
