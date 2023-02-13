using Business.Abstract;
using Business.DTOS;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public class PatientManager : IPatientService
    {
        IPatientDal _patientDal;

        public PatientManager(IPatientDal patientDal)
        {
            _patientDal = patientDal;
        }

        public void Add(Patient patient)
        {
            _patientDal.Add(patient);
        }

        public void Delete(Patient patient)
        {
            _patientDal.Delete(patient);
        }

        public Patient Get(Guid patientId)
        {
            return _patientDal.Get(p => p.PatientId == patientId);
        }

        public List<Patient> GetAll()
        {
            return _patientDal.GetAll();
        }

        public Patient GetByPhoneNumber(string phoneNumber)
        {
            return _patientDal.Get(q => q.PatientPhoneNumber == phoneNumber);
        }

        public async Task<PagingModelResponse<Patient>> PatientsDataTable(PatientDataTablesParam model)
        {
            var datas = await _patientDal.PatientDalDataTable(model.TextSearch, model.OrderCol, model.OrderDesc, model.Start, model.Size);

            return new PagingModelResponse<Patient>
            {
                ItemCount = datas.ItemCount,
                PageNumber = datas.PageNumber,
                PageSize = datas.PageSize,
                Model = datas.Model.Select(v => new Patient
                {
                    PatientId = v.PatientId,
                    PatientName = v.PatientName,
                    PatientLastName = v.PatientLastName,
                    PatientAge = v.PatientAge,
                    PatientPhoneNumber = v.PatientPhoneNumber,
                    PatientGender = v.PatientGender
                }).ToList()
            };
        }

        public void SetExistencePatientAsActive(Guid patientId, RegisterPatient patientNewData)
        {
            var patient = _patientDal.Get(q => q.PatientId == patientId);

            if (patient != null)
            {
                patient.IsUserActive = true;
                patient.PatientPassword = patientNewData.PatientPassword;
                patient.PatientName = patientNewData.PatientName;
                patient.PatientLastName = patientNewData.PatientLastName;
                patient.PatientPhotoPath = "profilephotos/default.png";
                patient.PatientNotificationToken = patientNewData.PatientNotificationToken;
                patient.PatientPhoneLanguage = patientNewData.PatientPhoneLanguage;
                _patientDal.Update(patient);
            }
        }

        public void SetPatientAsPassive(Guid patientId)
        {
            var patient = _patientDal.Get(q => q.PatientId == patientId);

            if (patient != null)
            {
                patient.IsUserActive = false;
                _patientDal.Update(patient);
            }
        }

        public void SetPatientPhoneLanguage(Guid patientId, string patientPhoneLanguage)
        {
            var patient = _patientDal.Get(q => q.PatientId == patientId);

            if (patient != null)
            {
                patient.PatientPhoneLanguage = patientPhoneLanguage;
                _patientDal.Update(patient);
            }
        }

        public void SetPatientTimeDifference(Guid patientId, int timeDifference)
        {
            var patient = _patientDal.Get(q => q.PatientId == patientId);

            if (patient != null)
            {
                patient.PatientTimeDifference = timeDifference;
                _patientDal.Update(patient);
            }
        }

        public void Update(Patient patient)
        {
            _patientDal.Update(patient);
        }

    }
}
