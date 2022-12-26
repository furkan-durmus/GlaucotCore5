using Business.DTOS;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMedicineService
    {
        List<Medicine> GetAll();
        List<string> GetAllMedicineName();
        Medicine Get(int medicineId);
        void Add(Medicine medicine);
        void Update(Medicine medicine);
        void Delete(Medicine medicine);
        Task<PagingModelResponse<Medicine>> MedicineDataTable(MedicineDataTablesParam model);
    }
}
