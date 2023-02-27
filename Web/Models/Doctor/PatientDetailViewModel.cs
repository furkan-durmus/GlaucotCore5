using System;
using System.Collections.Generic;

namespace Web.Models.Doctor
{
    public class PatientDetailViewModel
    {
        public List<MedicineInformation> Medicine { get; set; }
        public List<GlassRecordInformation> GlassRecord { get; set; }
        public List<EyePressure> EyePressure { get; set; }
    }

    public class MedicineInformation
    {
        public int MedicineRecordId { get; set; }
        public string MedicineName { get; set; }
        public int MedicineFrequency { get; set; }
        public string MedicineUsegeTimeList { get; set; }
        public string MedicineSideEffect { get; set; }
    }

    public class GlassRecordInformation
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public TimeSpan DiffDate { get; set; }
    }

    public class EyePressure
    {
        public string EyePressureDate { get; set; }
        public float LeftEyePressure { get; set; }
        public float RightEyePressure { get; set; }
    }

}
