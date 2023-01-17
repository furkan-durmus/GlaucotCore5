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
        IStaticDal _staticDal;

        public MobileHomeManager(IPatientDal patientDal, IMedicineRecordDal medicineRecordDal, IGlassRecordDal glassRecordDal, IStaticDal staticDal)
        {
            _patientDal = patientDal;
            _medicineRecordDal = medicineRecordDal;
            _glassRecordDal = glassRecordDal;
            _staticDal = staticDal;
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
            ApiHomePatientData apiHomePatientData = new ApiHomePatientData();
            if (patientData != null)
            {
                
                apiHomePatientData.PatientId = patientData.PatientId;
                apiHomePatientData.DoctorId = patientData.DoctorId;
                apiHomePatientData.PatientName = patientData.PatientName;
                apiHomePatientData.PatientLastName = patientData.PatientLastName;
                apiHomePatientData.PatientAge = patientData.PatientAge;
                apiHomePatientData.PatientGender = patientData.PatientGender;
                apiHomePatientData.PatientPhoneNumber = patientData.PatientPhoneNumber;
                apiHomePatientData.PatientPhotoPath = patientData.PatientPhotoPath;
                apiHomePatientData.PatientNotificationToken = patientData.PatientNotificationToken;
                apiHomePatientData.IsUserActive = patientData.IsUserActive;
                apiHomePatientData.IsGlassActive = patientGlassData != null ? patientGlassData.IsActive : false;
 
                List<Static> allStatics = _staticDal.GetAll();

                apiHomePatientData.AndroidMobileAppVersion = allStatics.FirstOrDefault(s=>s.Id == 1).StaticValue;
                apiHomePatientData.IosMobileAppVersion = allStatics.FirstOrDefault(s=>s.Id == 2).StaticValue;

            }
            return apiHomePatientData;
        }

        public List<MedicineRecord> GetUserDrugsData(Guid patientId)
        {
            return _medicineRecordDal.GetAll(m => m.PatientId == patientId).ToList();
        }

        public void UpdatePatientNotificationToken(GeneralMobilePatientRequest patient)
        {
            _patientDal.UpdatePatientNotificationToken(patient);
        }
    }
}
