using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public class DoctorManager : IDoctorService
    {
        IDoctorDal _doctorDal;

        public DoctorManager(IDoctorDal doctorDal)
        {
            _doctorDal = doctorDal;
        }

        public void Add(Doctor doctor)
        {
            _doctorDal.Add(doctor);
        }

        public void Delete(Doctor doctor)
        {
            _doctorDal.Delete(doctor);
        }

        public void DoctorApprove(Guid doctorId, bool confirmed)
        {
            Doctor response = _doctorDal.Get(q => q.DoctorId == doctorId);
            response.IsApproved = confirmed;

            _doctorDal.Update(response);
        }

        public Doctor Get(Guid doctorId)
        {
            return _doctorDal.Get(d => d.DoctorId == doctorId);
        }

        public List<Doctor> GetAll()
        {
            return _doctorDal.GetAll();
        }

        public Doctor GetByEmail(string email)
        {
            var response = _doctorDal.Get(q => q.DoctorEmail == email);
            return response;
        }

        public void Login(string email, string password)
        {
            var doctorUser = new Doctor { DoctorEmail = email, DoctorPassword = password };
            _doctorDal.Add(doctorUser);
        }

        public void Register(string email, string password, string securityStamp)
        {
            Doctor doctor = new Doctor
            {
                DoctorEmail = email,
                DoctorPassword = password,
                DoctorName = "GlaucoT",
                DoctorLastName = "GlaucoT",
                SecurityStamp = securityStamp,
                IsApproved = true
            };
            _doctorDal.Add(doctor);
        }

        public void Update(Doctor doctor)
        {
            _doctorDal.Update(doctor);
        }
    }
}
