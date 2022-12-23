using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Medicine : IEntity
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
    }
}
