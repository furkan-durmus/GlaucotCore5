using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Contrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public class MedicineRecordManager : IMedicineRecordService
    {
        IMedicineRecordDal _medicineRecordDal;

        public MedicineRecordManager(IMedicineRecordDal medicineRecordDal)
        {
            _medicineRecordDal = medicineRecordDal;
        }

        public int Add(MedicineRecord medicineRecord)
        {
            return _medicineRecordDal.AddWithReturnMedicineRecordId(medicineRecord);
        }

        public void Delete(MedicineRecord medicineRecord)
        {
            _medicineRecordDal.Delete(medicineRecord);
        }

        public MedicineRecord Get(int id)
        {
            return _medicineRecordDal.Get(m => m.MedicineRecordId == id);
        }

        public List<MedicineRecord> GetAll(Guid patientId)
        {
            return _medicineRecordDal.GetAll(m => m.PatientId == patientId);
        }

        public List<MedicineRecord> GetAllRecordsAccordingToTime(string time)
        {
            return _medicineRecordDal.GetAll(m => m.MedicineUsegeTimeList.Contains(time));
        }

        public void Update(MedicineRecord medicineRecord)
        {
            _medicineRecordDal.Update(medicineRecord);
        }
    }
}
