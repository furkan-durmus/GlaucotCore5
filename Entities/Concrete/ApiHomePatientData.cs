using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ApiHomePatientData
    {
        public UserGeneralData PatientGeneralData { get; set; }
        public List<UserMedicinesData> PatientMedicinesData { get; set; }
    }
}
