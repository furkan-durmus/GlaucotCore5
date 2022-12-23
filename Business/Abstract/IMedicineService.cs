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
        Medicine Get(int medicineId);
        void Add(Medicine medicine);
        void Update(Medicine medicine);
        void Delete(Medicine medicine);
    }
}
