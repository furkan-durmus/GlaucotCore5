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
    public class PatientManager : IPatientService
    {
        IPatientDal _patientDal;

        public PatientManager(IPatientDal patientDal)
        {
            _patientDal = patientDal;
        }

        public void Add(Patient patient)
        {
            _patientDal.Add(patient);
        }

        public void Delete(Patient patient)
        {
            _patientDal.Delete(patient);
        }

        public Patient Get(Guid patientId)
        {
            return _patientDal.Get(p => p.PatientId == patientId);
        }

        public List<Patient> GetAll()
        {
            return _patientDal.GetAll();
        }

        public void Update(Patient patient)
        {
            _patientDal.Update(patient);
        }

    }
}
