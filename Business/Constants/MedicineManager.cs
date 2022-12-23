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
    public class MedicineManager : IMedicineService
    {
        IMedicineDal _medicineDal;

        public MedicineManager(IMedicineDal medicineDal)
        {
            _medicineDal = medicineDal;
        }

        public void Add(Medicine medicine)
        {
            _medicineDal.Add(medicine);
        }

        public void Delete(Medicine medicine)
        {
            _medicineDal.Update(medicine);
        }

        public Medicine Get(int medicineId)
        {
            return _medicineDal.Get(m => m.MedicineId == medicineId);
        }

        public List<Medicine> GetAll()
        {
            return _medicineDal.GetAll();
        }

        public void Update(Medicine medicine)
        {
            _medicineDal.Update(medicine);
        }
    }
}
