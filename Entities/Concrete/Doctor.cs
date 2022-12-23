using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Doctor : IEntity
    {
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorLastName { get; set; }
        public string DoctorEmail { get; set; }
        public string DoctorPassword { get; set; }
        public string SecurityStamp { get; set; }
        public bool IsApproved { get; set; }
    }
}
