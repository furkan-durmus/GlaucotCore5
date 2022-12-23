using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMedicineRecordService
    {
        List<MedicineRecord> GetAll(Guid patientId);
        MedicineRecord Get(int id);
        int Add(MedicineRecord medicineRecord);
        void Update(MedicineRecord medicineRecord);
        void Delete(MedicineRecord medicineRecord);
        List<MedicineRecord> GetAllRecordsAccordingToTime(string time);

    }
}
