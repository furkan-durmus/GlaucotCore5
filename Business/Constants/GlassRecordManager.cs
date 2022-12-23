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
    public class GlassRecordManager : IGlassRecordService
    {
        IGlassRecordDal _glassRecordDal;

        public GlassRecordManager(IGlassRecordDal glassRecordDal)
        {
            _glassRecordDal = glassRecordDal;
        }

        public void Add(GlassRecord glassRecord)
        {
            _glassRecordDal.Add(glassRecord);
        }

        public void Delete(GlassRecord glassRecord)
        {
            _glassRecordDal.Delete(glassRecord);
        }

        public List<GlassRecord> GetAll(Guid patientId)
        {
            return _glassRecordDal.GetAll(g => g.PatientId == patientId);
        }

        public void Update(GlassRecord glassRecord)
        {
            _glassRecordDal.Update(glassRecord);
        }

        public string UpdateOrAddGlassRecord(Guid patientId)
        {
            GlassRecord lastGlassRecord = _glassRecordDal.GetLastRecordOfPatient(patientId);
            if (lastGlassRecord == null || lastGlassRecord.IsActive == false)
            {
                GlassRecord newGlassRecord = new();
                newGlassRecord.PatientId = patientId;
                newGlassRecord.StartDate = DateTime.Now;
                newGlassRecord.IsActive = true;

                _glassRecordDal.Add(newGlassRecord);

                return "Glass record successfully started";
            }
            else
            {

                lastGlassRecord.EndDate = DateTime.Now;
                lastGlassRecord.IsActive = false;

                _glassRecordDal.Update(lastGlassRecord);

                return "Glass record successfully ended";
            }
        }
    }
}
