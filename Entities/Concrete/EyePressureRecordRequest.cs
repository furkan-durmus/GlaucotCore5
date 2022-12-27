using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class EyePressureRecordRequest
    {
        public int Id { get; set; }
        public Guid PatientId { get; set; }
        public DateTime EyePressureDate { get; set; }
        public float LeftEyePressure { get; set; }
        public float RightEyePressure { get; set; }
        public string SecretKey { get; set; }
    }
}
