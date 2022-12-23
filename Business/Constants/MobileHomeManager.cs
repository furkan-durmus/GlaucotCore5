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
    public class MobileHomeManager : IMobileHomeService
    {
        IPatientDal _patientDal;
        IMedicineRecordDal _medicineRecordDal;
        IGlassRecordDal _glassRecordDal;

        public MobileHomeManager(IPatientDal patientDal, IMedicineRecordDal medicineRecordDal, IGlassRecordDal glassRecordDal)
        {
            _patientDal = patientDal;
            _medicineRecordDal = medicineRecordDal;
            _glassRecordDal = glassRecordDal;
        }

        public bool CheckKeyIsValid(Guid patientId, string key)
        {
            string expectedSecretKey = "";
            foreach (char character in patientId.ToString())
            {
                expectedSecretKey = expectedSecretKey + System.Convert.ToInt32(character);
            }

            string mykeyBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(expectedSecretKey));

            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(mykeyBase64);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                expectedSecretKey = Convert.ToHexString(hashBytes); // .NET 5 +
            }
            return expectedSecretKey == key ? true : false;
        }

        public ApiHomePatientData GetAllPatientDataForMobileHome(Guid patientId)
        {

            Patient patientData = _patientDal.Get(p => p.PatientId == patientId);
            GlassRecord patientGlassData = _glassRecordDal.GetLastRecordOfPatient(patientId);
            ApiHomePatientData apiHomePatientData = new();
            if (patientData != null)
            {
                UserGeneralData userGeneralData = new UserGeneralData();
                userGeneralData.PatientId = patientData.PatientId;
                userGeneralData.DoctorId = patientData.DoctorId;
                userGeneralData.PatientName = patientData.PatientName;
                userGeneralData.PatientLastName = patientData.PatientLastName;
                userGeneralData.PatientAge = patientData.PatientAge;
                userGeneralData.PatientGender = patientData.PatientGender;
                userGeneralData.PatientPhoneNumber = patientData.PatientPhoneNumber;
                userGeneralData.PatientPhotoPath = patientData.PatientPhotoPath;
                userGeneralData.IsUserActive = patientData.IsUserActive;
                userGeneralData.IsGlassActive = patientGlassData != null ? patientGlassData.IsActive : false;

                apiHomePatientData.PatientGeneralData = userGeneralData;
                apiHomePatientData.PatientMedicinesData = _medicineRecordDal.GetAllMedicineDataOfPatient(patientId);
            }
            return apiHomePatientData;
        }

        public List<MedicineRecord> GetUserDrugsData(Guid patientId)
        {
            return _medicineRecordDal.GetAll(m => m.PatientId == patientId).ToList();
        }

    }
}
