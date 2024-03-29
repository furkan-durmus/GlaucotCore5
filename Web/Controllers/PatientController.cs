﻿using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        IPatientService _patientService;
        IRegisterService _registerService;
        ILoginService _loginService;
        IMobileHomeService _mobileHomeService;
        IOTPService _otpService;
        IGlassRecordService _glassRecordService;
        IMedicineService _medicineService;
        IMedicineRecordService _medicineRecordService;
        IEyePressureRecordService _eyePressureRecordService;
        INotificationRecordService _notificationRecordService;
        public PatientController(IPatientService patientService, IRegisterService registerService, ILoginService loginService, IMobileHomeService mobileHomeService, IOTPService otpService, IGlassRecordService glassRecordService, IMedicineService medicineService, IMedicineRecordService medicineRecordService, IEyePressureRecordService eyePressureRecordService, INotificationRecordService notificationRecordService)
        {
            _patientService = patientService;
            _registerService = registerService;
            _loginService = loginService;
            _mobileHomeService = mobileHomeService;
            _otpService = otpService;
            _glassRecordService = glassRecordService;
            _medicineService = medicineService;
            _medicineRecordService = medicineRecordService;
            _eyePressureRecordService = eyePressureRecordService;
            _notificationRecordService = notificationRecordService;
        }

        [HttpPost("register")]

        public IActionResult RegisterPatient(RegisterPatient registerPatient)
        {

            if (!_registerService.CheckKeyIsValid(registerPatient))
            {
                return Ok(new { status = -99, message = $"Yetkisiz İşlem!" });
            }

            if (!_otpService.CheckOTP(registerPatient))
            {
                return Ok(new { status = 0, message = $"Geçersiz OTP" });
            }


            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(registerPatient.PatientPassword);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                registerPatient.PatientPassword = Convert.ToHexString(hashBytes); // .NET 5 +
            }



            if (_registerService.CheckPhoneIsExist(registerPatient.PatientPhoneNumber) && !_registerService.CheckUserIsActive(registerPatient.PatientPhoneNumber))
            {
                Patient existancePatient = _patientService.GetByPhoneNumber(registerPatient.PatientPhoneNumber);
                _patientService.SetExistencePatientAsActive(existancePatient.PatientId, registerPatient);
                return Ok(new { message = existancePatient.PatientId, status = 1 });
            }
            else
            {
                Patient patient = new();
                patient.DoctorId = new Guid("283EF1B0-BF5B-45A4-B27D-38AF07A9E2D5");
                patient.PatientId = new Guid();
                patient.PatientName = registerPatient.PatientName;
                patient.PatientLastName = registerPatient.PatientLastName;
                patient.PatientAge = 0;
                patient.PatientGender = 0;
                patient.PatientPhoneNumber = registerPatient.PatientPhoneNumber;
                patient.PatientPassword = registerPatient.PatientPassword;
                patient.PatientPhotoPath = "profilephotos/default.png";
                patient.IsUserActive = true;
                patient.PatientNotificationToken = registerPatient.PatientNotificationToken;
                patient.PatientPhoneLanguage = registerPatient.PatientPhoneLanguage;

                _patientService.Add(patient);
                return Ok(new { message = patient.PatientId, status = 1 });
            }
                


           
            
        }


        [HttpPost("login")]
        public IActionResult Login(LoginPatient loginPatient)
        {

            if (!_loginService.CheckKeyIsValid(loginPatient))
            {
                return Ok(new { status = -99, message = $"Yetkisiz İşlem!" });
            }

            if (!_loginService.CheckLoginIsValid(loginPatient))
            {
                return Ok(new { status = 0, message = $"Telefon numarası veya Şifre hatalı." });
            }
            return Ok(new { status = 1, message = _loginService.ResponsePatientId(loginPatient).PatientId });
        }

        [HttpPost("home")]
        public IActionResult PatientHomeData(GeneralMobilePatientRequest patient)
        {

            if (!_mobileHomeService.CheckKeyIsValid(patient.PatientId, patient.SecretKey))
            {
                return Ok(new { status = -99, message = $"Yetkisiz İşlem!" });
            }
            if (patient.PatientNotificationToken != null)
                _mobileHomeService.UpdatePatientNotificationToken(patient);
            if (patient.PatientTimeDifference != null)
                _patientService.SetPatientTimeDifference(patient.PatientId,patient.PatientTimeDifference ?? 0);
            if (patient.PatientPhoneLanguage != null)
                _patientService.SetPatientPhoneLanguage(patient.PatientId,patient.PatientPhoneLanguage);

            return Ok(new { status = 1, message = _mobileHomeService.GetAllPatientDataForMobileHome(patient.PatientId) });
        }

        [HttpPost("getPatientMedicineData")]
        public IActionResult PatientMedicineData(GeneralMobilePatientRequest patient)
        {

            if (!_mobileHomeService.CheckKeyIsValid(patient.PatientId, patient.SecretKey))
            {
                return Ok(new { status = -99, message = $"Yetkisiz İşlem!" });
            }
            if (patient.PatientNotificationToken != null)
                _mobileHomeService.UpdatePatientNotificationToken(patient);

            return Ok(new { status = 1, message = _medicineRecordService.GetAllWithDefaults(patient.PatientId) });
        }


        [HttpPost("getallmedicines")]
        public IActionResult GetAllMedicines(GeneralMobilePatientRequest patient)
        {

            if (!_mobileHomeService.CheckKeyIsValid(patient.PatientId, patient.SecretKey))
            {
                return Ok(new { status = -99, message = $"Yetkisiz İşlem!" });
            }

            return Ok(new { status = 1, message = _medicineService.GetAll() });
        }

        [HttpPost("setuseraspassive")]
        public IActionResult SetUserAsPassive(GeneralMobilePatientRequest patient)
        {

            if (!_mobileHomeService.CheckKeyIsValid(patient.PatientId, patient.SecretKey))
            {
                return Ok(new { status = -99, message = $"Yetkisiz İşlem!" });
            }
            _patientService.SetPatientAsPassive(patient.PatientId);
            return Ok(new { status = 1, message = "User set as passive." });
        }

        [HttpPost("addmedicinerecord")]
        public IActionResult AddPatientMedicineRecord(NewPatientMedicineRecord newMedicine)
        {

            if (!_mobileHomeService.CheckKeyIsValid(newMedicine.PatientId, newMedicine.SecretKey))
            {
                return Ok(new { status = -99, message = $"Yetkisiz İşlem!" });
            }

            MedicineRecord patientNewMedicineRecord = new();
            patientNewMedicineRecord.PatientId = newMedicine.PatientId;
            patientNewMedicineRecord.MedicineId = newMedicine.MedicineId;
            patientNewMedicineRecord.MedicineUsageRange = newMedicine.MedicineUsageRange;
            patientNewMedicineRecord.MedicineFrequency = newMedicine.MedicineFrequency;
            patientNewMedicineRecord.MedicineUsegeTimeList = newMedicine.MedicineUsegeTimeList;
            if (newMedicine.MedicineSideEffect != null)
                patientNewMedicineRecord.MedicineSideEffect = newMedicine.MedicineSideEffect;

            int medicineRecordId = _medicineRecordService.Add(patientNewMedicineRecord);


            return Ok(new { status = 1, message = medicineRecordId });
        }

        [HttpPost("updatemedicinerecord")]
        public IActionResult UpdatePatientMedicineRecord(NewPatientMedicineRecord newMedicineData)
        {

            if (!_mobileHomeService.CheckKeyIsValid(newMedicineData.PatientId, newMedicineData.SecretKey))
            {
                return Ok(new { status = -99, message = $"Yetkisiz İşlem!" });
            }

            MedicineRecord patientNewMedicineRecord = new();
            patientNewMedicineRecord.MedicineRecordId = newMedicineData.MedicineRecordId;
            patientNewMedicineRecord.PatientId = newMedicineData.PatientId;
            patientNewMedicineRecord.MedicineId = newMedicineData.MedicineId;
            patientNewMedicineRecord.MedicineUsageRange = newMedicineData.MedicineUsageRange;
            patientNewMedicineRecord.MedicineFrequency = newMedicineData.MedicineFrequency;
            patientNewMedicineRecord.MedicineUsegeTimeList = newMedicineData.MedicineUsegeTimeList;
            if (newMedicineData.MedicineSideEffect != null)
                patientNewMedicineRecord.MedicineSideEffect = newMedicineData.MedicineSideEffect;

            _medicineRecordService.Update(patientNewMedicineRecord);


            return Ok(new { status = 1, message = "Medicine record successfully updated." });
        }


        [HttpPost("deletemedicinerecord")]
        public IActionResult DeletePatientMedicineRecord(DeletePatientMedicineRecord MedicineDataForDelete)
        {

            if (!_mobileHomeService.CheckKeyIsValid(MedicineDataForDelete.PatientId, MedicineDataForDelete.SecretKey))
            {
                return Ok(new { status = -99, message = $"Yetkisiz İşlem!" });
            }

            MedicineRecord patientDeleteMedicineRecord = new();
            patientDeleteMedicineRecord.MedicineRecordId = MedicineDataForDelete.MedicineRecordId;
            patientDeleteMedicineRecord.PatientId = MedicineDataForDelete.PatientId;



            _medicineRecordService.Delete(patientDeleteMedicineRecord);


            return Ok(new { status = 1, message = "Medicine record successfully deleted." });
        }

        [HttpPost("addglassrecord")]
        public IActionResult AddPatientGlassRecord(GeneralMobilePatientRequest patient)
        {

            if (!_mobileHomeService.CheckKeyIsValid(patient.PatientId, patient.SecretKey))
            {
                return Ok(new { status = -99, message = $"Yetkisiz İşlem!" });
            }

            string response = _glassRecordService.UpdateOrAddGlassRecord(patient.PatientId);
            return Ok(new { status = response.Contains("start") ? 1 : 2, message = response });
        }

        [HttpPost("addeyepressurerecord")]
        public IActionResult AddPatientEyePressureRecord(EyePressureRecordRequest patientEyePressureRecordRequest)
        {

            if (!_mobileHomeService.CheckKeyIsValid(patientEyePressureRecordRequest.PatientId, patientEyePressureRecordRequest.SecretKey))
            {
                return Ok(new { status = -99, message = $"Yetkisiz İşlem!" });
            }

            EyePressureRecord eyePressureRecord = new();
            eyePressureRecord.PatientId = patientEyePressureRecordRequest.PatientId;
            eyePressureRecord.EyePressureDate = patientEyePressureRecordRequest.EyePressureDate;
            eyePressureRecord.LeftEyePressure = patientEyePressureRecordRequest.LeftEyePressure;
            eyePressureRecord.RightEyePressure = patientEyePressureRecordRequest.RightEyePressure;


            _eyePressureRecordService.AddPatientEyePressure(eyePressureRecord);
            return Ok(new { status = 1, message = "Göz Tansiyon Kaydı Başarılı" });
        }


        [HttpPost("sendregistersms")]
        public IActionResult PatientSendOTP(RegisterPatient registerPatient)
        {

            if (!_registerService.CheckKeyIsValid(registerPatient))
            {
                return Ok(new { status = -99, message = $"Yetkisiz İşlem!" });
            }

            if (!_otpService.CheckAcceptableSmsLimit(registerPatient))
            {
                return Ok(new { status = -1, message = $"Too many request" });
            }

            if (_registerService.CheckPhoneIsExist(registerPatient.PatientPhoneNumber) && _registerService.CheckUserIsActive(registerPatient.PatientPhoneNumber))
            {
                return Ok(new { status = 0, message = $"Bu telefon numarası ile kayıtlı bir hesabınız bulunuyor." });
            }
            OTP userOTPData = new();
            userOTPData.PatientPhoneNumber = registerPatient.PatientPhoneNumber;
            userOTPData.OTPCode = "12345";
            userOTPData.CreateDate = DateTime.Now;
            userOTPData.ExpireDate = DateTime.Now.AddMinutes(3);
            _otpService.Create(userOTPData);

            return Ok(new { status = 1, message = userOTPData.OTPCode });
        }

        [HttpPost("notificationrecordfeedback")]
        public IActionResult ApproveNotificationRecord(NotificationRecordResponse model)
        {

            if (!_mobileHomeService.CheckKeyIsValid(model.PatientId, model.SecretKey))
            {
                return Ok(new { status = -99, message = $"Yetkisiz İşlem!" });
            }

            bool isSucceed = false;
            if (model.NotificationRecordType == Core.NotificationRecordType.Approve)
                isSucceed = _notificationRecordService.ApproveNotificationRecord(model.NotificationRecordId);
            if (model.NotificationRecordType == Core.NotificationRecordType.Delay)
                isSucceed = _notificationRecordService.DelayNotificationRecord(model.NotificationRecordId);

            if (!isSucceed)
                return Ok(new { status = -99, message = $"Kayıt bulunamadı" });

            return Ok(new { status = 1, message = "İşlem başarılı" });
        }
    }
}