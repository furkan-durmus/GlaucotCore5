using System;
namespace Web.Identity
{
    public class DoctorUser
    {
        public Guid DoctorId { get; set; }
        public string DoctorEmail { get; set; }
        public string PasswordHashed { get; set; }
        public string SecurityStamp { get; set; }
    }
}

