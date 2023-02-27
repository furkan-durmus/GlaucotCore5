using Core.DataAccess.EntityFrameWork;
using Core.Entities;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contrete.EntityFramework
{
    public class EFMedicineRecordDal : EFEntityRepositoryBase<MedicineRecord, GlaucotContext>, IMedicineRecordDal
    {
        public int AddWithReturnMedicineRecordId(MedicineRecord record)
        {
            using (GlaucotContext context = new GlaucotContext())
            {
                var myEntity = context.Entry(record);
                myEntity.State = EntityState.Added;
                context.SaveChanges();
                return record.MedicineRecordId;
            }
        }

        public List<UserMedicinesData> GetAllMedicineDataOfPatient(Guid patientId)
        {
            using (GlaucotContext context = new GlaucotContext())
            {
                return (from medicineRecords in context.MedicineRecords
                        join medicines in context.Medicines on medicineRecords.MedicineId equals medicines.MedicineId
                        where medicineRecords.PatientId == patientId
                        select new UserMedicinesData
                        {
                            MedicineRecordId = medicineRecords.MedicineRecordId,
                            MedicineName = medicines.MedicineName,
                            MedicineFrequency = medicineRecords.MedicineFrequency,
                            MedicineSideEffect = medicineRecords.MedicineSideEffect,
                            MedicineUsageRange = medicineRecords.MedicineUsageRange,
                            MedicineUsegeTimeList =medicineRecords.MedicineUsegeTimeList,
                            MedicineImagePath = medicines.MedicineImagePath
                        }).ToList();
            }
        }

        public List<PatientMedicineRecordsWithDefaults> GetAllMedicineDataOfPatientWithDefaults(Guid patientId)
        {
            using (GlaucotContext context = new GlaucotContext())
            {
                return (from medicineRecords in context.MedicineRecords
                        join medicines in context.Medicines on medicineRecords.MedicineId equals medicines.MedicineId
                        where medicineRecords.PatientId == patientId
                        select new PatientMedicineRecordsWithDefaults
                        {
                            MedicineRecordId = medicineRecords.MedicineRecordId,
                            MedicineId = medicineRecords.MedicineId,
                            MedicineName = medicines.MedicineName,
                            MedicineFrequency = medicineRecords.MedicineFrequency,
                            MedicineSideEffect = medicineRecords.MedicineSideEffect,
                            MedicineUsageRange = medicineRecords.MedicineUsageRange,
                            MedicineUsegeTimeList = medicineRecords.MedicineUsegeTimeList,
                            MedicineImagePath = medicines.MedicineImagePath,
                            MedicineDefaultFrequency = medicines.MedicineDefaultFrequency,
                            MedicineDefaultTimeList = medicines.medicineDefaultTimeList
                            
                        }).ToList();
            }
        }

        public List<NotificationMedicine> GetDataForMedicineNotification(string time)
        {
            DateTime.TryParseExact(time, "HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime queryTime);

            using (GlaucotContext context = new GlaucotContext())
            {
                var myList = (from medicineRecords in context.MedicineRecords
                        join medicines in context.Medicines on medicineRecords.MedicineId equals medicines.MedicineId
                        join patients in context.Patients on medicineRecords.PatientId equals patients.PatientId
                        select new NotificationMedicine
                        {
                            MedicineName = medicines.MedicineName,
                            PatientNotificationToken = patients.PatientNotificationToken,
                            CurrentTime= queryTime.AddHours(patients.PatientTimeDifference ?? 0).ToString("HH:mm"),
                            PatientPhoneNumber=patients.PatientPhoneNumber,
                            PatientId = patients.PatientId,
                            PatientMedicineTimeList = medicineRecords.MedicineUsegeTimeList,
                            PatientTimeDifference = patients.PatientTimeDifference ?? 0,
                            PatientPhoneLanguage = patients.PatientPhoneLanguage,
                            MedicineRecordId = medicineRecords.MedicineRecordId
                        }).ToList();
                return myList.Where(q => q.PatientMedicineTimeList.Contains(queryTime.AddHours(q.PatientTimeDifference).ToString("HH:mm"))).ToList();
            }
        }
    }
}
