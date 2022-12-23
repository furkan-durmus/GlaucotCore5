using Business.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;

namespace Web.Jobs
{
    public class SendMedicineNotification
    {
        IMedicineRecordService _medicineRecordService;
        public SendMedicineNotification(IMedicineRecordService medicineRecordService)
        {
            _medicineRecordService = medicineRecordService;
        }
        public void SendNotificationWithOneSignal()
        {
            DateTime closestHalfOrFullTime = DateTime.Parse("16.12.2022 09:01:05");
            int minuteOfTime = closestHalfOrFullTime.Minute;
            if (minuteOfTime < 15)
                closestHalfOrFullTime = closestHalfOrFullTime.AddMinutes(-minuteOfTime).AddSeconds(-closestHalfOrFullTime.Second);
            else if (minuteOfTime < 30)
                closestHalfOrFullTime = closestHalfOrFullTime.AddMinutes((30- minuteOfTime)).AddSeconds(-closestHalfOrFullTime.Second);
            else if (minuteOfTime < 45)
                closestHalfOrFullTime = closestHalfOrFullTime.AddMinutes(-(minuteOfTime-30)).AddSeconds(-closestHalfOrFullTime.Second);
            else
                closestHalfOrFullTime = closestHalfOrFullTime.AddMinutes((60 - minuteOfTime)).AddSeconds(-closestHalfOrFullTime.Second);

            string myExactTime = closestHalfOrFullTime.ToString("HH:mm");

            List<MedicineRecord> patientsDataForNotification = _medicineRecordService.GetAllRecordsAccordingToTime(myExactTime);

        }
    }
}
