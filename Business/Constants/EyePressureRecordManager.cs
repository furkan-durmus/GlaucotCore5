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
    public class EyePressureRecordManager : IEyePressureRecordService
    {
        IEyePressureRecordDal _eyePressureRecordDal;

        public EyePressureRecordManager(IEyePressureRecordDal eyePressureRecordDal)
        {
            _eyePressureRecordDal = eyePressureRecordDal;
        }

        public void AddPatientEyePressure(EyePressureRecord eyePressureRecord)
        {
            _eyePressureRecordDal.Add(eyePressureRecord);
        }

        public List<EyePressureRecord> GetAllPatientEyePressure(Guid patientId)
        {
            return _eyePressureRecordDal.GetAll(p => p.PatientId == patientId);
        }
    }
}
