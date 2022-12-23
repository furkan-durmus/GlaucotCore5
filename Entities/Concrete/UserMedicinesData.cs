using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class UserMedicinesData
    {
        public int MedicineRecordId { get; set; }
        public string MedicineName { get; set; }
        public int MedicineUsageRange { get; set; }
        public int MedicineFrequency { get; set; }
        public string MedicineUsegeTimeList { get; set; }
        public string? MedicineSideEffect { get; set; }
    }
}
