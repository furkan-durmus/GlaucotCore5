﻿using Core.DataAccess;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IMedicineRecordDal : IEntitiyRepository<MedicineRecord>
    {
        List<UserMedicinesData> GetAllMedicineDataOfPatient(Guid patientId);
        int AddWithReturnMedicineRecordId(MedicineRecord record);
        List<NotificationMedicine> GetDataForMedicineNotification(string time);
        List<PatientMedicineRecordsWithDefaults> GetAllMedicineDataOfPatientWithDefaults(Guid patientId);
    }
}
