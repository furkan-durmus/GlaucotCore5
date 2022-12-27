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
        public string MedicineName { get; set; }
        public int MedicineFrequency { get; set; }
        public string MedicineUsegeTimeList { get; set; }
        public string MedicineSideEffect { get; set; }
    }

    public class GlassRecordInformation
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan DiffDate { get; set; }
    }

    public class EyePressure
    {
        public DateTime EyePressureDate { get; set; }
        public float LeftEyePressure { get; set; }
        public float RightEyePressure { get; set; }
    }

}
