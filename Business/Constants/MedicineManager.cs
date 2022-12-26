using Business.Abstract;
using Business.DTOS;
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

        public List<string> GetAllMedicineName()
        {
            return _medicineDal.GetAll().Select(q => q.MedicineName.ToLower()).ToList();
        }

        public async Task<PagingModelResponse<Medicine>> MedicineDataTable(MedicineDataTablesParam model)
        {
            var datas = await _medicineDal.MedicineDalDataTable(model.TextSearch, model.OrderCol, model.OrderDesc, model.Start, model.Size);

            return new PagingModelResponse<Medicine>
            {
                ItemCount = datas.ItemCount,
                PageNumber = datas.PageNumber,
                PageSize = datas.PageSize,
                Model = datas.Model.Select(v => new Medicine
                {
                    MedicineId = v.MedicineId,
                    MedicineName = v.MedicineName
                }).ToList()
            };
        }

        public void Update(Medicine medicine)
        {
            _medicineDal.Update(medicine);
        }
    }
}
