using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IPatientService
    {
        List<Patient> GetAll();
        Patient Get(Guid patientId);
        void Add(Patient patient);
        void Update(Patient patient);
        void Delete(Patient patient);
        
    }
}
