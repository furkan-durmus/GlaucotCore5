using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Patient : IEntity
    {
        public Guid   PatientId             { get; set; }
        public Guid   DoctorId              { get; set; }
        public string PatientName           { get; set; }
        public string PatientLastName       { get; set; }
        public int    PatientAge            { get; set; }
        public int    PatientGender         { get; set; }
        public string PatientPhoneNumber    { get; set; }
        public string PatientPassword       { get; set; }
        public string PatientPhotoPath      { get; set; }
        public bool   IsUserActive          { get; set; }
    }
}
