﻿using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMobileHomeService
    {
        bool CheckKeyIsValid(Guid patientId, string key);
        ApiHomePatientData GetAllPatientDataForMobileHome(Guid patientId);
        void UpdatePatientNotificationToken(GeneralMobilePatientRequest patient);
    }
}
